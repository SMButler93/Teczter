using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;
using Teczter.Adapters.DependencyInjection;
using Teczter.Infrastructure.Auth;
using Teczter.Services.DependencyInjection;
using Teczter.WebApi.Configurations;
using Teczter.WebApi.Middleware;
using Teczter.WebApi.Registrations;
using Teczter.WebApi.RequestValidations.ValidationAttributes;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContexts(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddIdentityCore<TeczterUser>(opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequireLowercase = true;
        opt.Password.RequireNonAlphanumeric = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

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

app.Run();
