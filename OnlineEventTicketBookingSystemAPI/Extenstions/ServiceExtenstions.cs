using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.Text;

namespace OnlineEventTicketBookingSystemAPI.Extenstions;

public static class ServiceExtenstions
{
    public static void ConfigureAddDbContext(this IServiceCollection service, 
        IConfiguration configuration)
    {
        service.AddDbContext<AppDbContext>(optins =>
        {
            optins.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        });
    }

    public static void ConfigureIdentityDbContext(this IServiceCollection service) =>
        service.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 5;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>();
        

    public static void ConfigureRepositoryPattern(this IServiceCollection service) =>
        service.AddScoped<IUnitOfWork, UnitOfWork>();


    public static void ConfigureJWTAuthentication(this IServiceCollection service,
        IConfiguration configuration)
    {
        JwtConfiguration jwtSettings = new();
        configuration.Bind(jwtSettings.Section, jwtSettings);
        
        service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.RequireHttpsMetadata = false;
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = jwtSettings.ValidAudience,
                ValidIssuer = jwtSettings.ValidIssuer,
                IssuerSigningKey = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
            };
        });
    }

    public static void AddJwtConfiguration(this IServiceCollection service,
        IConfiguration configuration)
    {
        service.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
    }

    public static void ConfigureCorsPolicy(this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            options.AddPolicy("corsPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
    }
}
