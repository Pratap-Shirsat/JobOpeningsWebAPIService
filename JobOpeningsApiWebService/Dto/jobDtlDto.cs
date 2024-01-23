using JobOpeningsApiWebService.Models;

namespace JobOpeningsApiWebService.Dto
{
	public class jobDtlDto
	{
		public int Id { get; set; }
		public string code { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public Location location { get; set; }
		public Department department { get; set; }
		public DateTime postedDate { get; set; }
		public DateTime closingDate { get; set; }
	}
}
