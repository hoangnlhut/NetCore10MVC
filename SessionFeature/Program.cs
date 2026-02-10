namespace SessionFeature
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IMySessionStorageEngine>(services => { 
                var path = Path.Combine(services.GetRequiredService<IHostEnvironment>().ContentRootPath, "sessions")  ;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return new FileMySessionStorageEngine(path);
            });
            builder.Services.AddSingleton<IMySessionStorage, MySessionStorage>();
            builder.Services.AddScoped<MySessionScopedContainer>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            

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
