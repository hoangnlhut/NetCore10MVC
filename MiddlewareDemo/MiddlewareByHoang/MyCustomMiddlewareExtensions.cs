namespace MiddlewareDemo.MiddlewareByHoang
{
    public static class MyCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyCustomeMiddleware>();
        }
    }
}
