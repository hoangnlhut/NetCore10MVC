using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using OidcDemo_OidcServer.Models;
using OidcDemo_OidcServer.Repository;

namespace OidcDemo_OidcServer.Controllers
{
    public class AuthorizeController : Controller
    {
        private static Random random = new Random();

        private readonly ILogger<AuthorizeController> logger;
        private readonly IUserRepository _userRepository;
        private readonly ICodeItemRepository _codeItemRepository;

        public AuthorizeController(
            IUserRepository userRepository,
            ICodeItemRepository codeItemRepository,
            ILogger<AuthorizeController> logger)
        {
            _userRepository = userRepository;
            _codeItemRepository = codeItemRepository;
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

            // creat authentication code
            var code = GenerateCode();

            // save authentication code to storage
            _codeItemRepository.Add(code, new CodeItem()
            {
                requestModel = authenticateRequest,
                user = user,
                scopes = scopes
            });


            return View("SubmitForm", model: new CodeFlowResponseViewModel()
            {
                Code = code,
                RedirectUri = authenticateRequest.RedirectUri,
                State = authenticateRequest.State
            });
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
