
namespace SessionFeature.MySession
{
    public static class MySesstionExtention
    {
        public const string SessionIdCookieName = "MY_SESSION_ID";

        public static ISession GetSession(this HttpContext httpContext)
        {
            var sessionContainer = httpContext.RequestServices.GetRequiredService<MySessionScopedContainer>();
            if (sessionContainer.Session != null)
            {
                return sessionContainer.Session;
            }
            else
            {
                string? sessionId = httpContext.Request.Cookies[SessionIdCookieName];
                ISession session = null!;
                if (IsSessionIdFormatValid(sessionId))
                {
                    session = httpContext.RequestServices.GetRequiredService<IMySessionStorage>().Get(sessionId!);
                }
                else
                {
                    session = httpContext.RequestServices.GetRequiredService<IMySessionStorage>().Create();
                }

                httpContext.Response.Cookies.Append(SessionIdCookieName, session.Id);
                sessionContainer.Session = session;
                return session;
            }
        }

        private static bool IsSessionIdFormatValid(string? sessionId)
        {
            return !string.IsNullOrEmpty(sessionId) && Guid.TryParse(sessionId, out _);
        }
    }
}
