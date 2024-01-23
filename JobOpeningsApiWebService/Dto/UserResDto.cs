using JobOpeningsApiWebService.Models;

namespace JobOpeningsApiWebService.Dto
{
	public class UserResDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Phone { get; set; }
		public userType UserType { get; set; } = userType.User;
	}
}
