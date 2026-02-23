using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MiddlewareDemo.ClientInfoRepository;
using MiddlewareDemo.Models;
namespace MiddlewareDemo.MiddlewareNamNet
{
    public class ClientInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ClientInfoMiddleware> _logger;

        public ClientInfoMiddleware(RequestDelegate next, ILogger<ClientInfoMiddleware> logger)
        {
            _next = next;
            _logger = logger;

            _logger.LogInformation("ClientInfoMiddleware is loaded.");    
        }
        public async Task InvokeAsync(HttpContext context, IClientInfoRepository clientInfoRepository)
        {
           
            var apiKey = context.Request.Headers["Api-Key"].FirstOrDefault();

            if (!string.IsNullOrEmpty(apiKey))
            {
                var clientInfo = clientInfoRepository.GetClientInfo(apiKey);
                if(clientInfo != null)
                {
                    _logger.LogInformation($"Found Api Key: {apiKey} with Client Info: {clientInfo.Name}");
                    context.Features.Set(clientInfo);
                    await _next(context);
                    return;
                }
            }

            _logger.LogInformation("Unauthorized: Invalid API Key");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid API Key");
        }
    }
}
