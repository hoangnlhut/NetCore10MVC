namespace HttpContextDemo.Extentions
{
    public static class HttpContextExtention
    {
        public static string GetDebugInfo(this HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        }
    }
}
