using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using SulamaSistemiWebApi.Data;
using SulamaSistemiWebApi.Services;
using SulamaSistemiWebApi.Interfaces;
using SulamaSistemiWebApi.Models;
using static BCrypt.Net.BCrypt;

var builder = WebApplication.CreateBuilder(args);

// Kestrel'i tüm IP adreslerini dinleyecek şekilde ayarla
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5093);
});

// CORS politikasını ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] 
            ?? throw new InvalidOperationException("JWT Key not found"))),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<TestDataService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sulama Sistemi Web API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed Data metodu
static void SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Veritabanını oluştur
    context.Database.EnsureCreated();
    
    // Kullanıcılar zaten varsa ekleme
    if (!context.Users.Any())
    {
        var users = new List<User>
        {
            new User
            {
                Username = "admin",
                Email = "admin@sulama.com",
                PasswordHash = HashPassword("admin123"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Username = "operator1",
                Email = "operator1@sulama.com",
                PasswordHash = HashPassword("operator123"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Username = "testuser",
                Email = "test@sulama.com",
                PasswordHash = HashPassword("test123"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };
        
        context.Users.AddRange(users);
        context.SaveChanges();
    }
    
    // Motorlar zaten varsa ekleme
    if (!context.Motors.Any())
    {
        var motors = new List<Motor>
        {
            new Motor
            {
                IsRunning = false,
                LastStatusChange = DateTime.UtcNow.AddHours(-2),
                LastOperationBy = "admin",
                LastOperationByUserId = 1
            },
            new Motor
            {
                IsRunning = true,
                LastStatusChange = DateTime.UtcNow.AddMinutes(-30),
                LastOperationBy = "operator1",
                LastOperationByUserId = 2
            },
            new Motor
            {
                IsRunning = false,
                LastStatusChange = DateTime.UtcNow.AddHours(-1),
                LastOperationBy = "admin",
                LastOperationByUserId = 1
            }
        };
        
        context.Motors.AddRange(motors);
        context.SaveChanges();
    }
    
    // Sıcaklık ve nem verileri zaten varsa ekleme
    if (!context.TemperatureHumidities.Any())
    {
        var random = new Random();
        var temperatureHumidities = new List<TemperatureHumidity>();
        
        // Son 24 saat için her saat başı veri
        for (int i = 24; i >= 0; i--)
        {
            temperatureHumidities.Add(new TemperatureHumidity
            {
                Temperature = 20 + random.Next(-5, 10) + (float)random.NextDouble(),
                Humidity = 40 + random.Next(-10, 20) + (float)random.NextDouble(),
                CreatedAt = DateTime.UtcNow.AddHours(-i)
            });
        }
        
        context.TemperatureHumidities.AddRange(temperatureHumidities);
        context.SaveChanges();
    }
    
    // Yağmur sensörü verileri zaten varsa ekleme
    if (!context.RainSensors.Any())
    {
        var random = new Random();
        var rainSensors = new List<RainSensor>();
        
        // Son 24 saat için her 2 saatte bir veri
        for (int i = 24; i >= 0; i -= 2)
        {
            rainSensors.Add(new RainSensor
            {
                RainLevel = random.Next(0, 100),
                CreatedAt = DateTime.UtcNow.AddHours(-i)
            });
        }
        
        context.RainSensors.AddRange(rainSensors);
        context.SaveChanges();
    }
    
    Console.WriteLine("Geçici veriler başarıyla oluşturuldu!");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS politikasını uygula
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Geçici verileri oluştur
SeedData(app);

app.Run();
