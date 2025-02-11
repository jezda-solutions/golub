using Golub.Contexts;
using Golub.Data;
using Golub.Email;
using Golub.Email.Providers;
using Golub.Endpoints;
using Golub.Interfaces.Repositories;
using Golub.Middlewares;
using Golub.Repositories;
using Golub.Responses;
using Golub.Responses.ProviderResponse;
using Golub.Services.ApiKeyServices;
using Golub.Services.Interfaces;
using Golub.Services.SeedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddLogging();

// Registrovanje servisa
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMigrations(connectionString);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmailProviderRepository, EmailProviderRepository>();
builder.Services.AddScoped<ISentEmailRepository, SentEmailRepository>();
builder.Services.AddScoped<IEmailSeedService, EmailSeedService>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<ApiKeyValidationService>();
builder.Services.AddScoped<IEmailProvider, ManDrillEmailProvider>();
builder.Services.AddScoped<IEmailProvider, SendGridEmailProvider>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailDistributor, EmailDistributor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Golub API",
        Version = "v1",
        Description = "API za slanje emailova"
    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-API-Key",
        Type = SecuritySchemeType.ApiKey,
        Description = "Unesite API ključ za autentifikaciju"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.Services.ApplyMigrations();

// Seedovanje podataka
using (var scope = app.Services.CreateScope())
{
    var emailSeedService = scope.ServiceProvider.GetRequiredService<IEmailSeedService>();
    await emailSeedService.SeedAsync();
}

// Registracija email endpointa
app.MapEmailEndpoints();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/emails"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeyMiddleware>();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Golub API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.Run();

