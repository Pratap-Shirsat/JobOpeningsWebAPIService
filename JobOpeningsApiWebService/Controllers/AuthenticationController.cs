using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Helpers;
using JobOpeningsApiWebService.Models;
using JobOpeningsApiWebService.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningsApiWebService.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly IConfiguration _configuration;
		private IUser _user;

		public AuthenticationController(IConfiguration configuration, IUser userRepo)
		{
			_configuration = configuration;
			_user = userRepo;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginReqDto model)
		{
			string secret = _configuration["JwtSettings:SecretKey"];

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			User? user = await _user.findUserByUserame(model.Username);
			if (user == null)
			{
				return Unauthorized("Invalid Credentials!");
			}

			if (!JwtHelper.VerifyPassword(model.Password, user.Password))
			{
				return Unauthorized("Invalid Credentials!");
			}

			var token = JwtHelper.GenerateJwtToken(secret, user);

			return Ok(new { Token = token });
		}
	}
}
