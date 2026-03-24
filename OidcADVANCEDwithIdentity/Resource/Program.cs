using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Resource.Helpers;

namespace Resource
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddControllersWithViews();

            var jwtBearerOptions = builder.Configuration.GetSection("JWTBearer").Get<JWTBearerOptions>() ?? throw new Exception("Could not get JWTBearerOptions");

            builder.Services.AddControllers();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = options.ClaimsIssuer;
                options.TokenHandlers.Add(new MyJsonWebTokenHandler());

                options.TokenValidationParameters.ValidIssuer = jwtBearerOptions.Issuer;
                options.TokenValidationParameters.ValidAudience = jwtBearerOptions.ClientId;
                options.TokenValidationParameters.ValidAlgorithms = new[] { "RS256" };
                options.TokenValidationParameters.IssuerSigningKey = JwkLoader.LoadFromPublic();
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("read", policy => policy.RequireClaim("read"));
                options.AddPolicy("write", policy => policy.RequireClaim("write"));
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
