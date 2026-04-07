using Ghtk.Api.AuthenticationHandler;
using Ghtk.Authorization;
using GhtkRepository;

namespace Ghtk.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var remoteAuthenServiceUrl = builder.Configuration["RemoteAuthenticationUrl"] ?? throw new Exception("missing IssueSigningKey configuration");
            var remoteAuthenticationService = new RemoteAuthenticationHandler(remoteAuthenServiceUrl);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthentication("X-Client-Source").AddXClientSourceAuthentication(
                options =>
                {
                    options.ValidateClientSource = (clientSource, token, principle) => remoteAuthenticationService.Validate(clientSource);
                    options.IssueSigningKey = builder.Configuration["IssueSigningKey"] ?? throw new Exception("missing IssueSigningKey configuration");
                }
            );
            builder.Services.AddMongoDbClient(builder.Configuration);
            builder.Services.AddScoped<IOrderRepository, MongoDbOrderRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
