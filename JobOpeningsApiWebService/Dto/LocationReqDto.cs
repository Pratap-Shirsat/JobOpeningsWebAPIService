using System.ComponentModel.DataAnnotations;

namespace JobOpeningsApiWebService.Dto
{
	public class LocationReqDto
	{
		[Required, MaxLength(255)]
		public string title { get; set; }

		[Required, MaxLength(50)]
		public string city { get; set; }

		[Required, MaxLength(50)]
		public string state { get; set; }

		[Required, MaxLength(50)]
		public string country { get; set; }

		[Required]
		public int zip { get; set; }
	}
}
