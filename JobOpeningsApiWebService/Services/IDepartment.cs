using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;

namespace JobOpeningsApiWebService.Services
{
	public interface IDepartment
	{
		Task<IEnumerable<Department>> GetDepartments();
		Task<Department?> GetDepartmentById(int id);
		Task<ResponseDto> AddDepartment(Department department);
		Task<ResponseDto> DeleteDepartment(int id);
		Task<ResponseDto> UpdateDepartment(Department department);
	}
}
