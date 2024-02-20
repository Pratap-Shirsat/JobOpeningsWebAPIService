using JobOpeningsApiWebService.Data;
using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningsApiWebService.Services
{
	public class UserRepository : IUser
	{
		private JobOpeningsDbContext _context;
		public UserRepository(JobOpeningsDbContext jobOpeningsDbContext)
		{
			_context = jobOpeningsDbContext;
		}

		public async Task<ResponseDto> AddUser(User user)
		{
			ResponseDto responseDto = new ResponseDto();

			if (await findUserByUserame(user.Username) != null)
			{
				responseDto.IsSuccess = false;
				responseDto.Message = $"User with username {user.Username} already exists!";
				return responseDto;
			}
			if (await findUserByEmail(user.Email) != null)
			{
				responseDto.IsSuccess = false;
				responseDto.Message = $"User with email {user.Email} already exists!";
				return responseDto;
			}
			if (await findUserByPhone(user.Phone) != null)
			{
				responseDto.IsSuccess = false;
				responseDto.Message = $"User with phone {user.Phone} already exists!";
				return responseDto;
			}
			var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
			var hashedPass = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
			user.Password = hashedPass;
			var result = await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			responseDto.Message = "Registered User successfully";
			return responseDto;
		}

		public async Task<User?> findUserByEmail(string email)
		{
			var user = await (from users in _context.Users where users.Email == email && users.IsDeleted == false select users).SingleOrDefaultAsync();
			return user;
		}

		public async Task<User?> findUserById(string id)
		{
			var usr = await (from user in _context.Users where user.Id == Guid.Parse(id) && user.IsDeleted == false select user).SingleOrDefaultAsync();
			return usr;
		}

		public async Task<User?> findUserByPhone(string phone)
		{
			var user = await (from users in _context.Users where users.Phone == phone && users.IsDeleted == false select users).SingleOrDefaultAsync();
			return user;
		}

		public async Task<User?> findUserByUserame(string name)
		{
			var userRes = await (from user in _context.Users where user.Username == name && user.IsDeleted == false select user).SingleOrDefaultAsync();
			return userRes;
		}

		public async Task<IEnumerable<User>> GetAllUsers()
		{
			var users = await (from usr in _context.Users where usr.IsDeleted == false select usr).ToListAsync();
			return users;
		}

		public async Task<ResponseDto> UpdateUser(User user)
		{
			ResponseDto responseDto = new();
			try
			{
				var usr = await _context.Users.FindAsync(user.Id);
				if (usr != null)
				{
					usr = user;
					await _context.SaveChangesAsync();
					responseDto.Message = "Updated User details successfully.";
					return responseDto;
				}
				responseDto.IsSuccess = false;
				responseDto.Message = $"User with ID {user.Id} not found";
				return responseDto;
			}
			catch (Exception ex)
			{
				responseDto.Message = ex.Message;
				responseDto.IsSuccess = false;
				return responseDto;
			}
		}
	}
}
