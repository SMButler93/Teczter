using FluentValidation;
using Microsoft.Extensions.Options;
using Serilog;
using Teczter.Adapters.DependencyInjection;
using Teczter.Services.DependencyInjection;
using Teczter.WebApi.Configurations;
using Teczter.WebApi.DemoModeConfigAndSetup;
using Teczter.WebApi.Middleware;
using Teczter.WebApi.Registrations;
using Teczter.WebApi.RequestValidations.ValidationAttributes;
using Teczter.WebApi.Setup;
using Teczter.WebApi.Setup.Registrations;

var builder = WebApplication.CreateBuilder(args);
var isDemoMode = builder.Configuration.GetValue<bool>("demo");

builder.Host.UseSerilog((context, services, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext();
});

// Add services to the container.
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<RequestValidationFilter>();
});

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();
builder.Services.RegisterAdapters();

builder.Services.AddOptions<CorsOptions>()
    .Bind(builder.Configuration.GetSection("CorsOptions"))
    .Validate(x => x.AllowedOrigins.Length > 0);

builder.Services.AddDbContexts(builder.Configuration, builder.Environment);

builder.Services.AddTeczterIdentity();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var corsOptions = app.Services.GetRequiredService<IOptions<CorsOptions>>().Value;

app.UseCors(corsBuilder =>
{
    corsBuilder.WithOrigins(corsOptions.AllowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseTeczterMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (isDemoMode)
{
    await app.SetupDemoMode();
} else
{
    await app.ApplyMigrations();
}

app.Run();
