using JobOpeningsApiWebService.Data;
using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using Microsoft.Data.SqlClient;
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
			try
			{
				var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
				var hashedPass = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
				user.Password = hashedPass;
				var result = await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();
				responseDto.Message = "Registered User successfully";
				return responseDto;
			}
			catch (DbUpdateException ex)
			{
				var sqlException = ex.InnerException as SqlException;
				responseDto.IsSuccess = false;
				responseDto.Message = sqlException?.Number == 2601 || sqlException?.Number == 2627 ? "Email/Username is already registered!" : ex.Message.ToString();
				return responseDto;
			}
		}

		public async Task<User?> findUserById(string id)
		{
			var usr = await (from user in _context.Users where user.Id == Guid.Parse(id) && user.IsDeleted == false select user).SingleOrDefaultAsync();
			return usr;
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
