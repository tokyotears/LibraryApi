using web.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace web.Services;

public class JwtService(IConfiguration config) {
	public string CreateJwtToken(User user) {
		var jwt = config.GetSection("Jwt");
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var claims = new[] {
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.Name),
			new Claim(ClaimTypes.Role, user.Role!.ToString())
		};
		var token = new JwtSecurityToken(
			issuer: jwt["Issuer"],
			audience: jwt["Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresMinutes"]!)),
			signingCredentials: creds
		);
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
