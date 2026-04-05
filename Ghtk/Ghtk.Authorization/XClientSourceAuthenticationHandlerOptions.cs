using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Ghtk.Authorization
{
    public class XClientSourceAuthenticationHandlerOptions : AuthenticationSchemeOptions
    {

        public Func<string, SecurityToken, ClaimsPrincipal, bool> ValidateClientSource { get; set; } = (clientSource, token, principle ) => false;

        public string IssueSigningKey { get; set; } = string.Empty;
    }
}
