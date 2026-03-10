using Microsoft.IdentityModel.Logging;
using System.Globalization;

namespace JWTDemo
{
    public class Program
    {
        private static IFormatProvider? enUs = new CultureInfo("en-Us");

        public static void Main(string[] args)
        {
            IdentityModelEventSource.ShowPII = true;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://mysso-server.com",
                        ValidAudience = "https://localhost:7117",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345superSecretKey@345"))
                    };

                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                Console.WriteLine("Token expired: " + context.Exception.Message);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("FinanceOnly", policy => policy.RequireClaim("department", "finance"));
                //a policy that requires some roles and claims
                options.AddPolicy("PolicyBoth12AndSomeClaims", policy => policy.RequireRole("Role1").RequireRole("Role2")
                .RequireClaim("client-id", "client1", "client2")
                .RequireUserName("Nguyen Van A")
                .RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth")
                .RequireAssertion(context => context.User.Identity?.Name?.Contains("Nguyen ", StringComparison.InvariantCultureIgnoreCase) ?? false)
                .RequireAssertion(context =>
                  DateTime.TryParseExact(context.User.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth").Select(x => x.Value).FirstOrDefault(), "yyyy-MM-dd", enUs, DateTimeStyles.None, out var dob)
                  && dob.Year < 2000)
                );

                options.AddPolicy("Policy3Or4", policy => policy.RequireRole("Role3", "Role4"));
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
