namespace SessionFeature.MySession
{
    public static class MySessionRegistrationExtentions
    {
        public static IServiceCollection AddMySession(this IServiceCollection services)
        {
            services.AddSingleton<IMySessionStorageEngine>(services => {
                var path = Path.Combine(services.GetRequiredService<IHostEnvironment>().ContentRootPath, "sessions");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return new FileMySessionStorageEngine(path);
            });
            services.AddSingleton<IMySessionStorage, MySessionStorage>();
            services.AddScoped<MySessionScopedContainer>();
            return services;
        }
    }
}
