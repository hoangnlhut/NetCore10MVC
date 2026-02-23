namespace MiddlewareDemo.MiddlewareByHoang
{
    public class MyCustomeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MyCustomeMiddleware> _logger;

        public MyCustomeMiddleware(RequestDelegate next, ILogger<MyCustomeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // IMessageWriter is injected into InvokeAsync
        public async Task InvokeAsync(HttpContext httpContext, IKeyData keyData)
        {
            // get data from keydata
            //var keyQuery = httpContext.Request.Query["key"];
            //if (!string.IsNullOrWhiteSpace(keyQuery))
            //{
            //    var dataExist = keyData.GetApiKeyData().ToList().FirstOrDefault(x => x.ApiKey == keyQuery);
            //    if (dataExist != null)
            //    {
            //        // Call the next delegate/middleware in the pipeline.
            //        _logger.LogInformation("Log successfully through MyCustomeMiddleware");
            //    }
            //    else
            //    {
            //        httpContext.Response.Redirect("/Home/Error");
            //    }
            //}

            var getRouteValue = httpContext.Request.RouteValues["action"]?.ToString();

            if (getRouteValue != null && getRouteValue == "GetUserByApiKey")
            {
                var getKeyApi = httpContext.Request.Query["keyApi"].ToString();
                                
                if (string.IsNullOrEmpty(getKeyApi)) 
                {
                    httpContext.Response.Redirect("/Home/Error");
                    return;
                }

                var dataExist = keyData.GetApiKeyData().ToList().FirstOrDefault(x => x.ApiKey == getKeyApi);
                if (dataExist != null)
                {
                    // Call the next delegate/middleware in the pipeline.
                    _logger.LogInformation($"Log successfully through MyCustomeMiddleware with User: {dataExist.User?.UserId} - {dataExist.User?.UserName}");
                }
                else
                {
                    httpContext.Response.Redirect("/Home/Error");
                }
            }


            await _next(httpContext);
        }
    }
}
