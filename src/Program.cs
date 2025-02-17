using Golub.Contexts;
using Golub.Data;
using Golub.Email;
using Golub.Email.Providers;
using Golub.Endpoints.Interfaces;
using Golub.Handlers.EmailProvider;
using Golub.Interfaces;
using Golub.Interfaces.Repositories;
using Golub.Middlewares;
using Golub.Repositories;
using Golub.Services.ApiKeyServices;
using Golub.Services.Interfaces;
using Golub.Services.SeedServices;
using Golub.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddLogging();

// Register services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMigrations(connectionString);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmailProviderRepository, EmailProviderRepository>();
builder.Services.AddScoped<ISentEmailRepository, SentEmailRepository>();
builder.Services.AddScoped<IEmailSeedService, EmailSeedService>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<ApiKeyValidationService>();
builder.Services.AddScoped<ApiKeyService>();
builder.Services.AddScoped<IEmailProvider, MandrillEmailProvider>();
builder.Services.AddScoped<IEmailProvider, SendGridEmailProvider>();
builder.Services.AddScoped<IEmailProvider, BrevoEmailProvider>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailDistributor, EmailDistributor>();

// Handlers registration
builder.Services.AddScoped<GetAllEmailProvidersHandler>();
builder.Services.AddScoped<GetEmailProviderByNameHandler>();
builder.Services.AddScoped<GetEmailProviderByIdHandler>();
builder.Services.AddScoped<AddEmailProviderHandler>();
builder.Services.AddScoped<UpdateEmailProviderHandler>();
builder.Services.AddScoped<UpdateEmailProviderIsActiveHandler>();
builder.Services.AddScoped<SoftDeleteEmailProviderHandler>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("Security"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Golub API",
        Version = "v1",
        Description = "API for sending emails"
    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-API-Key",
        Type = SecuritySchemeType.ApiKey,
        Description = "Enter API Key for authentication"
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

builder.Services.AddHealthChecks();

var app = builder.Build();

app.Services.ApplyMigrations();

// Seed email providers
using (var scope = app.Services.CreateScope())
{
    var emailSeedService = scope.ServiceProvider.GetRequiredService<IEmailSeedService>();
    await emailSeedService.SeedAsync();
}

// Register endpoints
var endpointDefinitions = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && typeof(IEndpoints).IsAssignableFrom(t))
    .Select(Activator.CreateInstance)
    .Cast<IEndpoints>();

foreach (var endpoint in endpointDefinitions)
{
    endpoint.RegisterEndpoints(app);
}

// Middleware checks the API Key only when the request starts with /api
app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
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

app.MapHealthChecks("/health");

app.Run();