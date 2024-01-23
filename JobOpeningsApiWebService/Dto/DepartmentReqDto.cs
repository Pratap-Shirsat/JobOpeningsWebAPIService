using System.ComponentModel.DataAnnotations;

namespace JobOpeningsApiWebService.Dto
{
	public class DepartmentReqDto
	{
		[Required, MaxLength(100)]
		public string title { get; set; }
	}
}
