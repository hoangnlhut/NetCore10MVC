using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
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
            var token = Request.Headers["Token"].FirstOrDefault();

            if (!string.IsNullOrEmpty(clientSource) && !string.IsNullOrEmpty(token)
                && VerifyClient(clientSource, token,out var principal))
            {
                //var identity = new ClaimsIdentity(Scheme.Name);
                //identity.AddClaim(new Claim(ClaimTypes.Name, clientSource!));
                //var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid verify"));
            }
        }

        private bool VerifyClient(string clientSource, string tokenValue, out ClaimsPrincipal? principle)
        {
            //validate token
            if (!VerifyJwtToken(tokenValue, out var token, out principle))
            {
                return false;
            }

            if (clientSource != (token as JwtSecurityToken)!.Subject)
            {
                return false;
            }

            if (!Options.ValidateClientSource(clientSource, token!, principle!))
            {
                return false;
            }

            return true;
        }

        private bool VerifyJwtToken(string tokenValue, out SecurityToken? token, out ClaimsPrincipal? principle)
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.IssueSigningKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
            try
            {
                principle = handler.ValidateToken(tokenValue, validationParameters, out token);
                return true;
            }
            catch(Exception)
            {
                token = null;
                principle = null;   
                return false;
            }
        }
    }
}
