using JobOpeningsApiWebService.Dto;
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
		public UsersController(IUser userRepo)
		{
			_user = userRepo;
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
		public async Task<IActionResult> Put(string id, [FromBody] UserReqDto userReqDto)
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
			user.Email = userReqDto.Email;
			user.Name = userReqDto.Name;
			user.Phone = userReqDto.Phone;
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
	}
}
