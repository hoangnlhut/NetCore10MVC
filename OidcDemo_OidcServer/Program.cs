using OidcDemo_OidcServer.Helpers;
using OidcDemo_OidcServer.Models;
using OidcDemo_OidcServer.Repository;

namespace OidcDemo_OidcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddSingleton<ICodeItemRepository, InMemoryCodeItemRepository>();

            var tokenIssuingOptions = builder.Configuration.GetSection("TokenIssuing").Get<TokenIssuingOptions>() ?? new TokenIssuingOptions();

            builder.Services.AddSingleton(tokenIssuingOptions);
            builder.Services.AddSingleton(JwkLoader.LoadFromDefault());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                Console.WriteLine($">>> Environment in SERVER: {app.Environment.EnvironmentName}");
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapGet("/.well-known/openid-configuration", () => Results.File(Path.Combine(builder.Environment.ContentRootPath, "OidcDiscovery", "openid-configuration.json"), contentType: "application/json"));

            app.MapGet("/.well-known/jwks.json", () => Results.File(Path.Combine(builder.Environment.ContentRootPath, "OidcDiscovery", "jwks.json"), contentType: "application/json"));

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                ;

            app.Run();
        }
    }
}
