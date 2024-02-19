using System.ComponentModel.DataAnnotations;

namespace JobOpeningsApiWebService.Dto
{
	public class PasswordResetReqDto
	{
		public string? Username { get; set; }

		[Phone]
		public string? Phone { get; set; }

		[EmailAddress]
		public string? Email { get; set; }

		public string? Password { get; set; }

		public bool IsCodeValidate { get; set; } = false;

		public string? Code { get; set; }
	}
}
