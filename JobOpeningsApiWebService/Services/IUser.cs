using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;

namespace JobOpeningsApiWebService.Services
{
	public interface IUser
	{
		Task<User?> findUserById(string id);
		Task<User?> findUserByUserame(string name);
		Task<ResponseDto> AddUser(User user);
		Task<ResponseDto> UpdateUser(User user);
		Task<IEnumerable<User>> GetAllUsers();
	}
}
