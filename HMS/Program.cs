using HMS.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Configure Identity for Auth Database
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

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
