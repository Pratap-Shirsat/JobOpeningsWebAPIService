using System.ComponentModel.DataAnnotations;

namespace JobOpeningsApiWebService.Dto
{
	public class UserUpdateReqDto
	{
		[MaxLength(100, ErrorMessage = "Name should be less then 100 characters.")]
		public string Name { get; set; }

		[EmailAddress, MaxLength(100)]
		public string Email { get; set; }

		[Phone]
		public string Phone { get; set; }

		[MaxLength(15, ErrorMessage = "Username should be less then 15 characters.")]
		public string Username { get; set; }
	}
}
