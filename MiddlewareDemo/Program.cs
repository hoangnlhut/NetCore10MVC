using MiddlewareDemo.ClientInfoRepository;
using MiddlewareDemo.MiddlewareByHoang;
using System.Globalization;

namespace MiddlewareDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IKeyData, MemoryKeyData>();
            builder.Services.AddSingleton<IClientInfoRepository, ClientInfoRepository.ClientInfoRepository>();

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

            //app.UseMyCustomMiddleware();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            //app.Use(async (context, next) =>
            //{
            //    var cultureQuery = context.Request.Query["culture"];
            //    if (!string.IsNullOrWhiteSpace(cultureQuery))
            //    {
            //        var culture = new CultureInfo(cultureQuery);

            //        CultureInfo.CurrentCulture = culture;
            //        CultureInfo.CurrentUICulture = culture;
            //    }

            //    // Call the next delegate/middleware in the pipeline.
            //    await next(context);
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(
            //        $"CurrentCulture.DisplayName: {CultureInfo.CurrentCulture.DisplayName}");
            //});

            app.Run();
        }
    }
}
