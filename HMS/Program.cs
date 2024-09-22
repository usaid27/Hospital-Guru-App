using HMS.DataContext;
using HMS.DataContext.Models;
using HMS.Dto;
using HMS.Repositories;
using HMS.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Retrieve configuration values
var useSqlLite = builder.Configuration.GetValue<bool>("UseSqlLite");
var SqlLiteAuthConnectionString = builder.Configuration.GetValue<string>("SqlLiteAuthConnectionString");
var SqlLiteDBConnectionString = builder.Configuration.GetValue<string>("SqlLiteDBConnectionString");

var MySqlAuthDbConnection = builder.Configuration.GetValue<string>("MySqlConnectionStrings:MySqlAuthDbConnection");
var MySqlDBConnection = builder.Configuration.GetValue<string>("MySqlConnectionStrings:MySqlDBConnection");

// Configure Entity Framework and Identity for Auth and App Databases
if (useSqlLite)
{
    builder.Services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseSqlite(SqlLiteAuthConnectionString);
        options.EnableSensitiveDataLogging(); // Enable detailed logging
        options.LogTo(Console.WriteLine); // Log EF Core SQL queries to console
    });

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlite(SqlLiteDBConnectionString);
        options.EnableSensitiveDataLogging(); // Enable detailed logging
        options.LogTo(Console.WriteLine); // Log EF Core SQL queries to console
    });
}
else
{
    builder.Services.AddDbContext<AuthDbContext>(options =>
    {
        options.UseMySql(MySqlAuthDbConnection, ServerVersion.AutoDetect(MySqlAuthDbConnection));
        options.EnableSensitiveDataLogging(); // Enable detailed logging
        options.LogTo(Console.WriteLine); // Log EF Core SQL queries to console
    });

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseMySql(MySqlDBConnection, ServerVersion.AutoDetect(MySqlDBConnection));
        options.EnableSensitiveDataLogging(); // Enable detailed logging
        options.LogTo(Console.WriteLine); // Log EF Core SQL queries to console
    });
}

// Read allowed origins from configuration
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

// Configure CORS to allow multiple domains
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policyBuilder => policyBuilder
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Configure EmailSender
builder.Services.AddTransient<IEmailSender>(provider =>
{
    var config = builder.Configuration.GetSection("EmailSettings");
    var smtpServer = config["SmtpHost"];
    var smtpPort = int.Parse(config["SmtpPort"]);
    var smtpUser = config["SmtpUser"];
    var smtpPass = config["SmtpPass"];
    var logger = provider.GetRequiredService<ILogger<EmailSender>>();
    return new EmailSender(smtpServer, smtpPort, smtpUser, smtpPass, logger);
});

// Configure Identity for Auth Database
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
    options.User.RequireUniqueEmail = true;
});


// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
// Bind MailSettings from configuration
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));
//builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddLogging();
builder.Services.AddScoped<BaseRepository>();
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Migrate Auth Database
        var authDbContext = services.GetRequiredService<AuthDbContext>();
        authDbContext.Database.Migrate(); // Applies any pending migrations for AuthDbContext

        // Migrate App Database
        var appDbContext = services.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate(); // Applies any pending migrations for AppDbContext
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();  // Enable authentication
app.UseAuthorization();

// Use CORS policy
app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
