using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobOpeningsApiWebService.Models
{
	public class Job
	{
		public int Id { get; set; }
		[Required, MaxLength(10)]
		public string code { get; set; }

		[Required, MaxLength(100)]
		public string title { get; set; }

		[Required, MaxLength(1500)]
		public string description { get; set; }

		public Location location { get; set; }
		[Required]
		public int locationId { get; set; }
		public Department department { get; set; }
		[Required]
		public int departmentId { get; set; }

		public DateTime postedDate { get; set; } = DateTime.Now.ToUniversalTime();

		[Required]
		public DateTime closingDate { get; set; }

		[JsonIgnore]
		public DateTime CreatedDate { get; set; } = DateTime.Now.ToUniversalTime();
		[JsonIgnore]
		public bool IsDeleted { get; set; } = false;
	}
}
