using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Teczter.Adapters.DependencyInjection;
using Teczter.Data;
using Teczter.Services.DependencyInjection;
using Teczter.WebApi.CorsConfig;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("TeczterDb"));
    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var corsOptions = app.Services.GetRequiredService<IOptions<CorsOptions>>().Value;

app.UseCors(policy =>
{
    policy.WithOrigins(corsOptions.AllowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
