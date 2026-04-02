using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Ghtk.Authorization
{
    public static class XClientSourceAuthenticationHandlerExtentions
    {
        public static AuthenticationBuilder AddXClientSourceAuthentication(this AuthenticationBuilder builder, Action<XClientSourceAuthenticationHandlerOptions> options)
        {
            return builder.AddScheme<XClientSourceAuthenticationHandlerOptions, XClientSourceAuthenticationHandler>("X-Client-Source", options);
        }
    }
}
