using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobOpeningsApiWebService.Models
{
	public class Department
	{
        public int Id { get; set; }

        [Required,MaxLength(100)]
        public string title { get; set; }

		[JsonIgnore]
		public DateTime CreatedDate { get; set; } = DateTime.Now;

		[JsonIgnore]
		public bool IsDeleted { get; set; } = false;
	}
}
