using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OidcServer.Helper;
using OidcServer.Models;
using OidcServer.Repository;
using System.Security.Claims;

namespace OidcServer.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly ILogger<AuthorizeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ICodeItemRepository _codeItemRepository;
        private readonly TokenIssuingOptions _tokenIssuingOptions;
        private readonly JsonWebKey _jsonWebKey;

        public AuthorizeController(
            IUserRepository userRepository,
            ICodeItemRepository codeItemRepository,
            TokenIssuingOptions tokenIssuingOptions,
            JsonWebKey jsonWebKey,
            ILogger<AuthorizeController> logger)
        {
            _logger = logger;
            _jsonWebKey = jsonWebKey;
            _userRepository = userRepository;
            _codeItemRepository = codeItemRepository;
            _tokenIssuingOptions = tokenIssuingOptions;
        }
        public IActionResult Index(AuthenticationRequestModel authenticateRequest)
        {
            ValidateAuthenticateRequestModel(authenticateRequest);
            return View(authenticateRequest);
        }

        [HttpPost]
        public IActionResult AuthorizeAsync(AuthenticationRequestModel authenticateRequest, string user, string[] scopes)
        {
            ArgumentNullException.ThrowIfNull(nameof(authenticateRequest));
            ArgumentException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNull(scopes);

            ValidateAuthenticateRequestModel(authenticateRequest);

            var userModel = _userRepository.FindByName(user);
            if (userModel == null)
            {
                return BadRequest("Invalid client_id");
            }

            string code = GenerateAuthenticationCode();
            var newCodeItem = new CodeItem()
            {
                Scopes = scopes,
                User = user,
                AuthenRequestModel = authenticateRequest
            };
            //save code 
            _codeItemRepository.Add(code, newCodeItem);

            _logger.LogInformation("New authentication code issued: {c}", code);

            return View("SubmitForm", new CodeFlowResponseViewModel()
            {
                Code = code,
                RedirectUri = authenticateRequest.RedirectUri,
                State = authenticateRequest.State,
            });
        }

        [HttpPost("oauth/token")]
        [ResponseCache(NoStore = true)] 
        //https://openid.net/specs/openid-connect-core-1_0.html#TokenEndpoint
        public IActionResult GetTokenAsync(string grant_type, string? code, string? refresh_token, string redirect_uri)
        {
            if (grant_type == "authorization_code") {

                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }

                var codeItem = _codeItemRepository.FindByCode(code);
                if (codeItem == null)
                {
                    return BadRequest("invalid code");
                }


                if (codeItem.AuthenRequestModel.RedirectUri != redirect_uri)
                {
                    return BadRequest();
                }

                _codeItemRepository.DeleteByCode(code); // code can not be reused

                var result = new AuthenticationResponseModel()
                {
                    AccessToken = GenerateAccessToken(codeItem.User, string.Join(' ', codeItem.Scopes), codeItem.AuthenRequestModel.ClientId, codeItem.AuthenRequestModel.Nonce, _jsonWebKey),
                    IdToken = GenerateIdToken(codeItem.User, codeItem.AuthenRequestModel.ClientId, codeItem.AuthenRequestModel.Nonce, _jsonWebKey),
                    TokenType = "Bearer",
                    RefreshToken = GenerateRefreshCode(),
                    ExpiresIn = 1200 // valid in 20 minutes
                };

                _logger.LogInformation("access_token: {t}", result.AccessToken);
                _logger.LogInformation("refresh_token: {t}", result.RefreshToken);

                return Json(result);
            }
            else
            {
                return BadRequest();

            }
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

        private static string GenerateRefreshCode()
        {
            return GenerateAuthenticationCode();
        }

        private static string GenerateAuthenticationCode()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
