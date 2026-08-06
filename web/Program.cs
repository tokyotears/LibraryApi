using web.Utilities;
using web.Data;
using web.Endpoints;
using web.Services;
using web.Middleware;
using web.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddScoped<JwtService>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapGet("/", Utilities.Greeting);
app.MapUserEndpoints();
app.MapLibEndpoints();

app.Run();
