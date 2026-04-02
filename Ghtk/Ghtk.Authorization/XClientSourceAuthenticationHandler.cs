using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Ghtk.Authorization
{
    public class XClientSourceAuthenticationHandler : AuthenticationHandler<XClientSourceAuthenticationHandlerOptions>
    {
        public XClientSourceAuthenticationHandler(IOptionsMonitor<XClientSourceAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var clientSource = Request.Headers["X-Client-Source"].FirstOrDefault();
            if (string.IsNullOrEmpty(clientSource))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing X-Client-Source header"));
            }

            if(!Options.ValidateClientSource(clientSource))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid X-Client-Source header"));
            }

            var identity = new ClaimsIdentity(Scheme.Name);
            identity.AddClaim(new Claim(ClaimTypes.Name, clientSource));
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
