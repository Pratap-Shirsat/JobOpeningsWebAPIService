using JobOpeningsApiWebService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobOpeningsApiWebService.Helpers
{
	public static class JwtHelper
	{
		public static string GenerateJwtToken(string secretKey, User user)
		{
			var claims = new List<Claim>
			{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.Username),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role,user.UserType.ToString()),
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: null, // Set your issuer if applicable
				audience: null, // Set your audience if applicable
				claims: claims,
				expires: DateTime.UtcNow.AddHours(1), // Token expiration time
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public static bool VerifyPassword(string userPassword, string hashedPassword)
		{
			bool isValid = BCrypt.Net.BCrypt.Verify(userPassword, hashedPassword);
			return isValid;
		}
	}

}