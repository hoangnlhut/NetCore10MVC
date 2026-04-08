using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Apis;
using MinimalAPI.Bootstrapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MinmalToDoApp")));

builder.Services.AddRepositoryServices();

var app = builder.Build();

app.MapToDoItemApi();

app.Run();
