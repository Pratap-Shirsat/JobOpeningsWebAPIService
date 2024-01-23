using JobOpeningsApiWebService.Data;
using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningsApiWebService.Services
{
	public class DepartmentRepository : IDepartment
	{
		private JobOpeningsDbContext _context;
		public DepartmentRepository(JobOpeningsDbContext dbContext)
		{
			_context = dbContext;
		}

		public async Task<ResponseDto> AddDepartment(Department department)
		{
			await _context.Departments.AddAsync(department);
			await _context.SaveChangesAsync();
			return new ResponseDto();
		}

		public async Task<ResponseDto> DeleteDepartment(int id)
		{
			var dept = await _context.Departments.FindAsync(id);
			ResponseDto res = new();
			if (dept == null)
			{
				res.IsSuccess = false;
				res.Message = $"Department with id {id} not found.";
				return res;
			}
			dept.IsDeleted = true;
			await _context.SaveChangesAsync();
			res.Message = $"Department with id {id} deleted successfully!";
			return res;
		}

		public async Task<Department?> GetDepartmentById(int id)
		{
			var dept = await (from depts in _context.Departments where depts.Id == id && depts.IsDeleted == false select depts).FirstOrDefaultAsync();
			return dept;
		}

		public async Task<IEnumerable<Department>> GetDepartments()
		{
			return await (from depts in _context.Departments where depts.IsDeleted == false select depts).ToListAsync();
		}

		public async Task<ResponseDto> UpdateDepartment(Department department)
		{
			var dept = await (from depts in _context.Departments where depts.IsDeleted == false && depts.Id == department.Id select depts).FirstOrDefaultAsync();
			ResponseDto res = new();
			if (dept == null)
			{
				res.IsSuccess = false;
				res.Message = $"Department with id {department.Id} not found.";
				return res;
			}
			dept.title = department.title;
			await _context.SaveChangesAsync();
			res.Message = $"Department with id {dept.Id} Updated successfully!";
			return res;
		}
	}
}
