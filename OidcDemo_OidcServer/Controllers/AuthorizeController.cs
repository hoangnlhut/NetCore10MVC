using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OidcDemo_OidcServer.Helpers;
using OidcDemo_OidcServer.Models;
using OidcDemo_OidcServer.Repository;
using System.Security.Claims;

namespace OidcDemo_OidcServer.Controllers
{
    public class AuthorizeController : Controller
    {
        private static Random random = new Random();

        private readonly ILogger<AuthorizeController> logger;
        private readonly IUserRepository _userRepository;
        private readonly ICodeItemRepository _codeItemRepository;
        private readonly TokenIssuingOptions _tokenIssuingOptions; 

        public AuthorizeController(
            IUserRepository userRepository,
            ICodeItemRepository codeItemRepository,
            TokenIssuingOptions tokenIssuingOptions,
            ILogger<AuthorizeController> logger)
        {
            _userRepository = userRepository;
            _codeItemRepository = codeItemRepository;
            _tokenIssuingOptions = tokenIssuingOptions;
            this.logger = logger;
        }

        public IActionResult Index(AuthenticationRequestModel authenticationRequestModel)
        {
            ValidateAuthenticateRequestModel(authenticationRequestModel);
            return View(authenticationRequestModel);
        }

        [HttpPost]
        public IActionResult AuthorizeAsync(AuthenticationRequestModel authenticateRequest, string user, string[] scopes)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNull(scopes);

            ValidateAuthenticateRequestModel(authenticateRequest);

            if (_userRepository.FindByUsername(user) is null)
            {
                return View("UserNotFound");
            }

            // creat authentication code
            var code = GenerateCode();

            // save authentication code to storage
            _codeItemRepository.Add(code, new CodeItem()
            {
                RequestModel = authenticateRequest,
                User = user,
                Scopes = scopes
            });

            var codeViewModel = new CodeFlowResponseViewModel()
            {
                Code = code,
                RedirectUri = authenticateRequest.RedirectUri,
                State = authenticateRequest.State
            };


            return View("SubmitForm", codeViewModel);
        }

        // https://openid.net/specs/openid-connect-core-1_0.html#TokenEndpoint: have to insert 
        // string grant_type, string? code, string? refresh_token, string redirect_uri params below
        [Route("oauth/token")]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public IActionResult ReturnTokens(string grant_type, string? code, string? refresh_token, string redirect_uri)
        {
            #region Validate Input
            if (grant_type != "authorization_code" || string.IsNullOrEmpty(code))
            {
                return BadRequest();
            }

            // get code item from storage by code, if not exist return bad request
            var codeItem = _codeItemRepository.FindByCode(code);
            if (codeItem == null)
            {
                return BadRequest();
            }

            //Ensure that the redirect_uri parameter value is identical to the redirect_uri parameter value that was included in the initial Authorization Request.
            if (codeItem.RequestModel.RedirectUri != redirect_uri)
            {
                return BadRequest();
            }
            #endregion

            //delete item code from storage due to the code can only be used once, if not exist return bad request
            _codeItemRepository.Delete(code);

            var jwk = JwkLoader.LoadFromDefault();

            var model = new AuthenticationResponseModel()
            {
                AccessToken = GenerateAccessToken(codeItem.User, string.Join(' ',  codeItem.Scopes), codeItem.RequestModel.ClientId, codeItem.RequestModel.Nonce, jwk),
                IdToken = GenerateIdToken(codeItem.User, codeItem.RequestModel.ClientId, codeItem.RequestModel.Nonce, jwk),
                TokenType = "Bearer",
                RefreshToken = GenerateRefreshToken(),
                ExpiresIn = 1200 // valid in 20 minutes
            };


            return Json(model);
        }

        private static string GenerateRefreshToken()
        {
            return GenerateCode();
        }

        private string GenerateAccessToken(string userId, string scope, string audience, string nonce, JsonWebKey jsonWebKey)
        {
            // access_token can be the same as id_token, but here we might have different values for expirySeconds so we use 2 different functions

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new("scope", scope) // Jeg vet ikke hvorfor JwtRegisteredClaimNames inneholder ikke "scope"??? Det har kun OIDC ting?  https://datatracker.ietf.org/doc/html/rfc8693#name-scope-scopes-claim
            };
            var idToken = JwtGenerator.GenerateJWTToken(
                _tokenIssuingOptions.AccessTokenExpirySeconds,
                _tokenIssuingOptions.Issuer,
                audience,
                nonce,
                claims,
                jsonWebKey
                );

            return idToken;
        }

        private string GenerateIdToken(string userId, string audience, string nonce, JsonWebKey jsonWebKey)
        {
            // https://openid.net/specs/openid-connect-core-1_0.html#IDToken
            // we can return some claims defined here: https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId)
            };

            var idToken = JwtGenerator.GenerateJWTToken(
                _tokenIssuingOptions.IdTokenExpirySeconds,
                _tokenIssuingOptions.Issuer,
                audience,
                nonce,
                claims,
                jsonWebKey
                );


            return idToken;
        }

        private static void ValidateAuthenticateRequestModel(AuthenticationRequestModel authenticateRequest)
        {
            ArgumentNullException.ThrowIfNull(authenticateRequest, nameof(authenticateRequest));

            if (string.IsNullOrEmpty(authenticateRequest.ClientId))
            {
                throw new Exception("client_id required");
            }

            if (string.IsNullOrEmpty(authenticateRequest.ResponseType))
            {
                throw new Exception("response_type required");
            }

            if (string.IsNullOrEmpty(authenticateRequest.Scope))
            {
                throw new Exception("scope required");
            }

            if (string.IsNullOrEmpty(authenticateRequest.RedirectUri))
            {
                throw new Exception("redirect_uri required");
            }
        }


        private static string GenerateCode()
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(alphabet, 32)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }


    }
}
