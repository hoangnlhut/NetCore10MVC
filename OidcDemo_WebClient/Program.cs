using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                // Optional cookie settings
                options.Cookie.Name = "oidc_web_client_cookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, openIdOptions =>
            {
                // Basic settings
                openIdOptions.Authority = authority;
                openIdOptions.ClientId = clientId;
                openIdOptions.ClientSecret = clientSecret;
                openIdOptions.ResponseType = OpenIdConnectResponseType.Code;
                openIdOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                openIdOptions.SaveTokens = true; // Persist tokens in authentication session
                openIdOptions.GetClaimsFromUserInfoEndpoint = true;
                openIdOptions.MapInboundClaims = false;
                
                openIdOptions.TokenValidationParameters.NameClaimType = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Name;
                openIdOptions.TokenValidationParameters.RoleClaimType = "roles";

                // Scopes
                openIdOptions.Scope.Clear();
                openIdOptions.Scope.Add("openid");
                openIdOptions.Scope.Add("profile");
                openIdOptions.Scope.Add("email");
                if (!string.IsNullOrWhiteSpace(extraScopes))
                {
                    foreach (var s in extraScopes.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!string.Equals(s, "openid", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(s, "profile", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(s, "email", StringComparison.OrdinalIgnoreCase))
                        {
                            openIdOptions.Scope.Add(s);
                        }
                    }
                }
                // offline_access to enable refresh tokens if supported
                if (!openIdOptions.Scope.Contains("offline_access"))
                {
                    openIdOptions.Scope.Add("offline_access");
                }

                openIdOptions.TokenValidationParameters.ValidateIssuerSigningKey = false;
                openIdOptions.TokenValidationParameters.SignatureValidator = delegate (string token, TokenValidationParameters validationParameters)
                {
                    return new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(token);
                };

                //openIdOptions.TokenValidationParameters.ValidIssuer = authority;
                //openIdOptions.TokenValidationParameters.ValidAudience = clientId;
                //openIdOptions.TokenValidationParameters.ValidAlgorithms = new[] { "RS256" };
                //openIdOptions.TokenValidationParameters.IssuerSigningKey = JwkLoader.LoadFromPublic();

                openIdOptions.Events.OnAuthorizationCodeReceived = (context) =>
                {
                    Console.WriteLine($"authorization_code: {context.ProtocolMessage.Code}");

                    return Task.CompletedTask;
                };

                openIdOptions.Events.OnTokenResponseReceived = (context) =>
                {
                    Console.WriteLine($"OnTokenResponseReceived.access_token: {context.TokenEndpointResponse.AccessToken}");
                    Console.WriteLine($"OnTokenResponseReceived.refresh_token: {context.TokenEndpointResponse.RefreshToken}");

                    return Task.CompletedTask;
                };

                openIdOptions.Events.OnTokenValidated = (context) =>
                {
                    Console.WriteLine($"OnTokenValidated.access_token: {context.TokenEndpointResponse?.AccessToken}");
                    Console.WriteLine($"OnTokenValidated.refresh_token: {context.TokenEndpointResponse?.RefreshToken}");

                    return Task.CompletedTask;
                };

                openIdOptions.Events.OnTicketReceived = (context) =>
                {
                    return Task.CompletedTask;
                };

                // Optional: events for logging or custom behavior
                openIdOptions.Events = new OpenIdConnectEvents
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

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Authentication must come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
