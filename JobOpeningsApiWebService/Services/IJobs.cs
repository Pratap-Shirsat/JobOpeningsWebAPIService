using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;

namespace JobOpeningsApiWebService.Services
{
	public interface IJobs
	{
		Task<IEnumerable<JobResponseDto>> GetJobs(string q, int? locId, int? deptId);
		Task<jobDtlDto?> GetJobById(int id);
		Task<ResponseDto> AddJob(Job job);
		Task<ResponseDto> DeleteJob(int id);
		Task<ResponseDto> UpdateJob(Job job);
	}
}
