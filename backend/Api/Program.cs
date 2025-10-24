using Api.Configuration;
using Api.Extensions.ServiceExtensions;
using Api.Extensions;
using Api.Services;
using Logic.Database;
using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.ConfigureCorsForDevelopment();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureAppSwagger();

builder.Services.ConfigureInvalidModelStateResponse();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.ConfigureAndAddIdentity<User, IdentityRole<Guid>, AppDbContext>();
builder.Services.ConfigureAppAuth(builder.Configuration);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, Sha256HashingService>();
builder.Services.AddScoped<IUserContextService, HttpUserContextService>();
builder.Services.AddScoped<IInvoiceGenerationService, InvoiceGenerationService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using var scope = app.Services.CreateScope();
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.AddSwaggerForDevelopment();
app.ApplyCorsForDevelopment();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run("http://0.0.0.0:5000");
