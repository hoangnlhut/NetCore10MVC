using Microsoft.IdentityModel.Logging;

namespace JWTDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IdentityModelEventSource.ShowPII = true;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
                {
                    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345superSecretKey@345")) { KeyId = "my-key-id" };

                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        //TO_DO: For development only, set these to false. In production, you should validate these parameters. Some of these parameters should not be hardcoded, but SHOULD BE READ from configuration.
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://mysso-server.com",
                        ValidAudience = "https://localhost:7117",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345superSecretKey@345"))
                    };

                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if(context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                Console.WriteLine("Token expired: " + context.Exception.Message);
                            }
                            return Task.CompletedTask;
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

            //app.UseAuthentication();
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
