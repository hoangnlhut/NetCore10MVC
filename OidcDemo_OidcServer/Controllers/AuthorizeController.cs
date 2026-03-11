using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using OidcDemo_OidcServer.Models;
using OidcDemo_OidcServer.Repository;

namespace OidcDemo_OidcServer.Controllers
{
    public class AuthorizeController : Controller
    {
        public const int TokenResponseValidSeconds = 1200;
        public const int CodeResponseValidSeconds = 60 * 5;

        private readonly ILogger<AuthorizeController> logger;
        private readonly IUserRepository _userRepository;
        //private readonly TokenIssuingOptions tokenIssuingOptions;
        //private readonly JsonWebKey jsonWebKey;
        //private readonly ICodeStorage codeStorage;
        //private readonly IRefreshTokenStorageFactory refreshTokenStorageFactory;
        //private readonly IAuthorizationClientService authorizationClientService;

        public AuthorizeController(
            //TokenIssuingOptions tokenIssuingOptions,
            //JsonWebKey jsonWebKey,
            //ICodeStorage codeStorage,
            //IRefreshTokenStorageFactory refreshTokenStorageFactory,
            //IAuthorizationClientService authorizationClientService,
            IUserRepository userRepository,
            ILogger<AuthorizeController> logger)
        {
            //this.tokenIssuingOptions = tokenIssuingOptions;
            //this.jsonWebKey = jsonWebKey;
            //this.codeStorage = codeStorage;
            //this.refreshTokenStorageFactory = refreshTokenStorageFactory;
            //this.authorizationClientService = authorizationClientService;

            _userRepository = userRepository;
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

            if(_userRepository.FindByUsername(user) is null)
            {
                return View("UserNotFound");
            }

            //var client = authorizationClientService.FindById(authenticateRequest.ClientId);
            //if (client == null)
            //{
            //    return BadRequest("Invalid client_id");
            //}

            //if (client.RedirectUri != authenticateRequest.RedirectUri)
            //{
            //    return BadRequest("Invalid redirect_uri");
            //}

            //string code = GenerateAuthenticationCode();
            //if (!codeStorage.TryAddCode(code, new CodeStorageValue()
            //{
            //    Code = code,
            //    ClientId = authenticateRequest.ClientId,
            //    OriginalRedirectUri = authenticateRequest.RedirectUri,
            //    ExpiryTime = DateTime.Now.AddSeconds(CodeResponseValidSeconds),
            //    Nonce = authenticateRequest.Nonce,
            //    User = user,
            //    Scope = authenticateRequest.Scope
            //}
            //))
            //{
            //    throw new Exception("Error storing code");
            //}

            //var codeFlowModel = BuildCodeFlowResponseModel(authenticateRequest, code);

            //string viewName = "SubmitForm"; // we can change to another view if we need to support response_modes other than form_post

            //logger.LogInformation("New authentication code issued: {c}", code);

            //return View(viewName, new CodeFlowResponseViewModel()
            //{
            //    Code = codeFlowModel.Code,
            //    RedirectUri = authenticateRequest.RedirectUri,
            //    State = codeFlowModel.State,
            //});
            return View();
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
        private static string GenerateAuthenticationCode()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
