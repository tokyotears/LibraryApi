using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Enums;
using web.DTO;
using web.Interfaces;
using web.Services;
using web.Repositories;
using web.Validation;

namespace web.AppConfig;

public static class AppConfig
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Library API",
                Description = "API for library system management",
            });
        });
       
        var jwtSection = config.GetSection("Jwt");
        var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ClockSkew = TimeSpan.FromSeconds(30)
            };
            options.Events = new JwtBearerEvents {
                OnMessageReceived = context => {
                    var token = context.Request.Cookies["jwt_token"];
                    if (!string.IsNullOrEmpty(token)) context.Token = token;
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILibRepository, LibRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<LibService>();
        services.AddScoped<BookService>();
        services.AddScoped<IValidator<UserAuthRequest>, UserAuthReqValidator>();
        services.AddScoped<IValidator<UserCreateRequest>, UserCreateReqValidator>();
        services.AddScoped<IValidator<UserUpdateRequest>, UserUpdateReqValidator>();
        services.AddScoped<IValidator<LibCreateRequest>, LibCreateReqValidator>();
        services.AddScoped<IValidator<LibUpdateRequest>, LibUpdateReqValidator>();
        services.AddScoped<IValidator<BookCreateRequest>, BookCreateReqValidator>();
        services.AddScoped<IValidator<BookUpdateRequest>, BookUpdateReqValidator>();
        services.AddScoped<IRequestValidationDispatcher, RequestValidationDispatcher>();
        services.AddScoped<JwtService>();

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<Roles>());
        });

        return services;
    }
}