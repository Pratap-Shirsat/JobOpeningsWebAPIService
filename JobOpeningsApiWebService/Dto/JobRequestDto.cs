using System.ComponentModel.DataAnnotations;

namespace JobOpeningsApiWebService.Dto
{
	public class JobRequestDto
	{

		[Required, MaxLength(100)]
		public string title { get; set; }

		[Required, MaxLength(1500)]
		public string description { get; set; }

		[Required]
		public int locationId { get; set; }

		[Required]
		public int departmentId { get; set; }

		[Required]
		public DateTime closingDate { get; set; }
	}
}
