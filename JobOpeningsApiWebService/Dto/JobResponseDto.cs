using JobOpeningsApiWebService.Models;

namespace JobOpeningsApiWebService.Dto
{
	public class JobResponseDto
	{
		public int Id { get; set; }
		public string code { get; set; }
		public string title { get; set; }
		public string location { get; set; }
		public string department { get; set; }
		public DateTime postedDate { get; set; }
		public DateTime closingDate { get; set; }
	}
}
