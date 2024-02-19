using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobOpeningsApiWebService.Models
{
	public class Location
	{
		public int Id { get; set; }

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

		[JsonIgnore]
		public DateTime CreatedDate { get; set; } = DateTime.Now.ToUniversalTime();

		[JsonIgnore]
		public bool IsDeleted { get; set; } = false;
	}
}
