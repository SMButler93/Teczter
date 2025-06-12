using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Teczter.Adapters.DependencyInjection;
using Teczter.Data;
using Teczter.Services.DependencyInjection;
using Teczter.WebApi.Middleware;
using Teczter.WebApi.MiddlewareAndConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext();
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();
builder.Services.RegisterAdapters();

builder.Services.AddOptions<CorsOptions>()
    .Bind(builder.Configuration.GetSection("CorsOptions"));

builder.Services.AddDbContext<TeczterDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TeczterDb"))
    .LogTo(Console.WriteLine)
    .UseLazyLoadingProxies();

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var corsOptions = app.Services.GetRequiredService<IOptions<CorsOptions>>().Value;

app.UseCors(builder =>
{
    builder.WithOrigins(corsOptions.AllowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseTeczterMiddleware();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
