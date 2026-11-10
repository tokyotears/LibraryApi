using web.Utilities;
using web.Data;
using web.Endpoints;
using web.Services;
using web.Middleware;
using web.Validation;
using web.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.SwaggerDoc("v1", new OpenApiInfo {
		Version = "v1",
		Title = "Library API",
		Description = "An ASP.NET Core Web API for managing a library system.",
	});
});
builder.Services.AddControllers();

var jwtSection = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
builder.Services.AddAuthentication(options => {
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
builder.Services.AddAuthorization();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILibRepository, LibRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILibService, LibService>();
builder.Services.AddScoped<UserAuthReqValidator>();
builder.Services.AddScoped<LibAddReqValidator>();
builder.Services.AddScoped<BookAddReqValidator>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
