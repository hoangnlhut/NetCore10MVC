using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;

namespace OidcDemo_WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if(builder.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var openIdConfig = builder.Configuration.GetSection("OpenId");
            var authority = openIdConfig["Authority"] ?? "";
            var clientId = openIdConfig["ClientId"] ?? "";
            var clientSecret = openIdConfig["ClientSecret"] ?? "";
            var extraScopes = openIdConfig["Scopes"] ?? "";

            // Configure authentication: cookie + OpenID Connect (authorization code flow)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                // Optional cookie settings
                options.Cookie.Name = "oidc_web_client_cookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // Basic settings
                options.Authority = authority;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.SaveTokens = true; // Persist tokens in authentication session
                options.GetClaimsFromUserInfoEndpoint = true;
                options.MapInboundClaims = false;

                options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                options.TokenValidationParameters.RoleClaimType = "roles";

                // Scopes
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                if (!string.IsNullOrWhiteSpace(extraScopes))
                {
                    foreach (var s in extraScopes.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.Equals(s, "openid", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(s, "profile", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(s, "email", StringComparison.OrdinalIgnoreCase))
                        {
                            options.Scope.Add(s);
                        }
                    }
                }
                // offline_access to enable refresh tokens if supported
                if (!options.Scope.Contains("offline_access"))
                {
                    options.Scope.Add("offline_access");
                }


                // Optional: events for logging or custom behavior
                options.Events = new OpenIdConnectEvents
                {
                    OnRemoteFailure = ctx =>
                    {
                        // Let the pipeline handle failures (could redirect to error page)
                        ctx.HandleResponse();
                        ctx.Response.Redirect("/Home/Error");
                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Authentication must come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
