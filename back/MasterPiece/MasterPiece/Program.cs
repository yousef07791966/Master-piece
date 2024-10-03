using DinkToPdf.Contracts;
using DinkToPdf;
using MasterPiece.DTO;
using MasterPiece.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// add this to program.cs  first step when i start to the project
// builder.Services.AddDbContext<MyDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));
//// add this to program.cs  first step when i start to the project



//// program.cs for jwt
//// Note 1  Before var app = builder.Build();
//// Register TokenGenerator as a singleton or transient service
//builder.Services.AddSingleton<TokenGeneratorDTO>(); // or .AddTransient<TokenGenerator>()
//// Retrieve JWT settings from configuration
//var jwtSettings = builder.Configuration.GetSection("Jwt");
//var key = jwtSettings.GetValue<string>("Key");
//var issuer = jwtSettings.GetValue<string>("Issuer");
//var audience = jwtSettings.GetValue<string>("Audience");
//// Ensure values are not null
//if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
//{
//    throw new InvalidOperationException("JWT settings are not properly configured.");
//}
//// Add JWT Authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        var jwtSettings = builder.Configuration.GetSection("Jwt");
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Issuer"],
//            ValidAudience = jwtSettings["Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
//        };
//    });
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
//});// program.cs for jwt


//// Configure CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("Development", builder =>
//    {
//        builder.AllowAnyOrigin() // Fixed URL format (removed extra https://)
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//// Configure the HTTP request pipeline.for jwt
//app.UseAuthentication(); // new
//app.UseAuthorization();
//app.UseCors("Development");

//app.MapControllers();

//app.Run();

/// /////////////////////////////////////////////////////////////////////
/// /////////////////////////////////////////////////////////////////////
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", builder =>
    {
        builder.AllowAnyOrigin() // Fixed URL format (removed extra https://)
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure DbContext with SQL Server
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));

// Configure Serilog logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Clothes.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


////////////////////////////////////////////////////////////////////////////////
///JWT Token
///
// Add JWT Token generation service
builder.Services.AddSingleton<TokenGeneratorDTO>();
builder.Services.AddScoped<PayPalPaymentService>();

// Retrieve JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings.GetValue<string>("Key");
var issuer = jwtSettings.GetValue<string>("Issuer");
var audience = jwtSettings.GetValue<string>("Audience");

if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new InvalidOperationException("JWT settings are not properly configured.");
}

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// Configure Authorization with custom policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});
////////////////////////////////////////////////////////////////////////////////
///                                      JWT Token end
///////////////////////////////////////////////////////////////////////////////
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(provider =>
    new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Enable CORS
app.UseCors("Development");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Enable JWT authentication middleware
app.UseAuthorization();  // Enable Authorization middleware

app.MapControllers();
app.Run();