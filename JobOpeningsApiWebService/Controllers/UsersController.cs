using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Helpers;
using JobOpeningsApiWebService.Models;
using JobOpeningsApiWebService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningsApiWebService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private IUser _user;
		private readonly IConfiguration _configuration;
		public UsersController(IUser userRepo, IConfiguration configuration)
		{
			_user = userRepo;
			_configuration = configuration;
		}

		// GET: api/<UsersController>
		[HttpGet]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Get()
		{
			var users = await _user.GetAllUsers();
			IList<UserResDto> res = new List<UserResDto>();

			foreach (var user in users)
			{
				UserResDto userRes = new UserResDto();
				userRes.UserType = user.UserType;
				userRes.Name = user.Name;
				userRes.Email = user.Email;
				userRes.Username = user.Username;
				userRes.Id = user.Id.ToString();
				userRes.Phone = user.Phone;
				res.Add(userRes);
			}
			return Ok(res);
		}

		// GET api/<UsersController>/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> Get(string id)
		{
			var user = await _user.findUserById(id);
			if (user == null)
			{
				return NotFound($"User with id {id} not found!");
			}
			UserResDto userRes = new();
			userRes.Username = user.Username;
			userRes.Id = user.Id.ToString();
			userRes.Email = user.Email;
			userRes.Name = user.Name;
			userRes.Phone = user.Phone;

			return Ok(userRes);
		}

		// User Register
		// POST api/<UsersController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UserReqDto userReq)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			User user = new();
			user.Email = userReq.Email;
			user.Password = userReq.Password;
			user.Username = userReq.Username;
			user.Name = userReq.Name;
			user.Phone = userReq.Phone;

			var res = await _user.AddUser(user);
			if (res.IsSuccess)
			{
				return StatusCode(StatusCodes.Status201Created, res.Message);
			}
			return BadRequest(res.Message);
		}

		// PUT api/<UsersController>/5
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> Put(string id, [FromBody] UserUpdateReqDto userReqDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var user = await _user.findUserById(id);
			if (user == null)
			{
				return BadRequest($"User with id {id} not found.");
			}
			if (user.Email != userReqDto.Email)
			{
				var userByEmail = await _user.findUserByEmail(userReqDto.Email);
				if (userByEmail != null)
				{
					return BadRequest($"Email address ${userReqDto.Email} is already being used.");
				}
				user.Email = userReqDto.Email;
			}
			if (user.Username != userReqDto.Username)
			{
				var userByUsername = await _user.findUserByUserame(userReqDto.Username);
				if (userByUsername != null)
				{
					return BadRequest($"Username {userReqDto.Username} is already taken.");
				}
				user.Username = userReqDto.Username;
			}
			if (user.Phone != userReqDto.Phone)
			{
				var userByPhone = await _user.findUserByPhone(userReqDto.Phone);
				if (userByPhone != null)
				{
					return BadRequest($"Phone number {userReqDto.Phone} is been used by another account.");
				}
				user.Phone = userReqDto.Phone;
			}
			user.Name = userReqDto.Name;
			var result = await _user.UpdateUser(user);
			if (result.IsSuccess)
			{
				return Ok(result.Message);
			}
			return BadRequest(result.Message);
		}

		// DELETE api/<UsersController>/5
		[HttpDelete("{id}")]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _user.findUserById(id);
			if (user == null)
			{
				return BadRequest($"User with id {id} not found.");
			}
			user.IsDeleted = true;
			var result = await _user.UpdateUser(user);
			if (result.IsSuccess)
			{
				return Ok(result.Message);
			}
			return BadRequest(result.Message);
		}

		[HttpPost("reset_password")]
		public async Task<IActionResult> ResetPassword([FromBody] PasswordResetReqDto passwordResetReq)
		{
			User user = null;
			if (passwordResetReq.Username?.Trim().Length > 0)
			{
				user = await _user.findUserByUserame(passwordResetReq.Username);
			}
			else if (passwordResetReq.Email?.Trim().Length > 0)
			{
				user = await _user.findUserByEmail(passwordResetReq.Email);
			}
			else if (passwordResetReq.Phone?.Trim().Length > 0)
			{
				user = await _user.findUserByPhone(passwordResetReq.Phone);
			}
			else
			{
				return BadRequest("Username, Email or Phone is required!");
			}

			if (user == null)
			{
				return BadRequest("User account does not exists!");
			}

			if (passwordResetReq.IsCodeValidate)
			{
				if (user.ResetCode?.Trim().Length == 6 && user.ResetCode == passwordResetReq.Code)
				{
					user.Password = user.ResetPassword;
					user.ResetPassword = string.Empty;
					user.ResetCode = string.Empty;
					var res = await _user.UpdateUser(user);
					if (res.IsSuccess)
					{

						return Ok("Password has been successfully reset.");
					}
					else
					{
						return BadRequest(res.Message);
					}
				}
				else
				{
					return BadRequest("Invalid verification code!");
				}
			}
			else
			{
				if (passwordResetReq.Password != null && passwordResetReq.Password.Trim().Length < 5)
				{
					return BadRequest("Invalid reset password!");
				}
				string code = generateVerifyCode();
				var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
				var hashedPass = BCrypt.Net.BCrypt.HashPassword(passwordResetReq.Password.Trim(), salt);
				user.ResetPassword = hashedPass;
				user.ResetCode = code;
				var res = await _user.UpdateUser(user);
				if (res.IsSuccess)
				{
					EmailSender emailSender = new EmailSender(_configuration);
					string recipientEmail = user.Email;
					string subject = "Verification code for Reset Password - JobOpeningsService";
					string body = $"The verification code to reset the password is {code}";

					emailSender.SendEmail(recipientEmail, subject, body);

					return Ok($"A code has been sent to the registered email address {user.Email}.");
				}
				else
				{
					return BadRequest(res.Message);
				}
			}
		}

		private string generateVerifyCode()
		{
			Random random = new Random();
			string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			char[] code = new char[6];

			for (int i = 0; i < 6; i++)
			{
				code[i] = allowedChars[random.Next(0, allowedChars.Length)];
			}

			return new string(code);
		}
	}
}
