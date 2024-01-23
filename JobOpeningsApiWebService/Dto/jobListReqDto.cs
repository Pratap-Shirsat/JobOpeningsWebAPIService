namespace JobOpeningsApiWebService.Dto
{
	public class jobListReqDto
	{
		public string q { get; set; } = string.Empty;
		public int pageNo { get; set; } = 1;
		public int pageSize { get; set; } = 10;
		public int? locationId { get; set; }
		public int? departmentId { get; set; }
	}
}
