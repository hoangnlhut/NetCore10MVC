namespace ClientAuthentication.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("SqlConnection") ?? throw new Exception("missing DefaultConnection configuration");

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddTransient<IClientSourceAuthenticationHandler>(_ => new SqlServerClientSourceAuthenticationHandler(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
