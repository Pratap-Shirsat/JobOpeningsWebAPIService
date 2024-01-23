using JobOpeningsApiWebService.Data;
using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningsApiWebService.Services
{
	public class JobRepository : IJobs
	{
		private JobOpeningsDbContext _context;
		public JobRepository(JobOpeningsDbContext jobOpeningsDbContext)
		{
			_context = jobOpeningsDbContext;
		}
		public async Task<ResponseDto> AddJob(Job job)
		{
			ResponseDto res = new();
			var loc = await (from locations in _context.Locations where locations.IsDeleted == false && locations.Id == job.locationId select locations).FirstOrDefaultAsync();
			if (loc == null)
			{
				res.IsSuccess = false;
				res.Message = $"Location with id {job.locationId} does not exists";
				return res;
			}

			var dept = await (from depts in _context.Departments where depts.IsDeleted == false && depts.Id == job.departmentId select depts).FirstOrDefaultAsync();
			if (dept == null)
			{
				res.Message = $"Department with id {job.departmentId} does not exists!";
				res.IsSuccess = false;
				return res;
			}
			var results = await _context.Jobs.AddAsync(job);
			await _context.SaveChangesAsync();
			res.Message = $"Created Job successfully";
			return res;
		}

		public async Task<ResponseDto> DeleteJob(int id)
		{
			ResponseDto res = new();
			var job = await (from jobs in _context.Jobs where jobs.Id == id && jobs.IsDeleted == false select jobs).FirstOrDefaultAsync();
			if (job == null)
			{
				res.IsSuccess = false;
				res.Message = $"Job with id {id} does not exists.";
				return res;
			}
			job.IsDeleted = true;
			await _context.SaveChangesAsync();
			res.Message = $"Job with id {id} has been deleted.";
			return res;
		}

		public async Task<jobDtlDto?> GetJobById(int id)
		{
			var job = await _context.Jobs
									.Where(jobs => jobs.Id == id && jobs.IsDeleted == false)
									.Include(jobs => jobs.department).Include(jobs => jobs.location).FirstOrDefaultAsync();
			if (job == null)
			{
				return null;
			}

			jobDtlDto jb = new jobDtlDto();
			jb.Id = id;
			jb.title = job.title;
			jb.description = job.description;
			jb.department = job.department;
			jb.location = job.location;
			jb.closingDate = job.closingDate;
			jb.postedDate = job.postedDate;
			jb.code = job.code;
			return jb;
		}

		public async Task<IEnumerable<JobResponseDto>> GetJobs(string q, int? locId, int? deptId)
		{
			List<Job> jobs = new();

			if (locId != null && deptId != null)
			{
				jobs = await _context.Jobs
									.Where(jobs => jobs.IsDeleted == false && jobs.locationId == locId && jobs.departmentId == deptId)
									.Include(jobs => jobs.department).Include(jobs => jobs.location).ToListAsync();
			}
			else if (locId != null)
			{
				jobs = await _context.Jobs
									.Where(jobs => jobs.IsDeleted == false && jobs.locationId == locId)
									.Include(jobs => jobs.department).Include(jobs => jobs.location).ToListAsync();
			}
			else if (deptId != null)
			{
				jobs = await _context.Jobs
									.Where(jobs => jobs.IsDeleted == false && jobs.departmentId == deptId)
									.Include(jobs => jobs.department).Include(jobs => jobs.location).ToListAsync();
			}
			else
			{
				jobs = await _context.Jobs
									.Where(jobs => jobs.IsDeleted == false)
									.Include(jobs => jobs.department).Include(jobs => jobs.location).ToListAsync();
			}

			var filteredJobs = q.Length != 0 ? jobs.Where(j => j.title.ToUpper().Contains(q.ToUpper())) : jobs;

			List<JobResponseDto> jobsList = new List<JobResponseDto>();
			foreach (var job in filteredJobs)
			{
				JobResponseDto jobDto = new JobResponseDto();
				jobDto.Id = job.Id;
				jobDto.title = job.title;
				jobDto.code = job.code;
				jobDto.postedDate = job.postedDate;
				jobDto.closingDate = job.closingDate;
				jobDto.location = job.location.title;
				jobDto.department = job.department.title;
				jobsList.Add(jobDto);
			}
			return jobsList;
		}

		public async Task<ResponseDto> UpdateJob(Job job)
		{
			ResponseDto responseDto = new ResponseDto();
			var jb = await (from jobs in _context.Jobs where jobs.IsDeleted == false && jobs.Id == job.Id select jobs).FirstOrDefaultAsync();
			if (jb == null)
			{
				responseDto.IsSuccess = false;
				responseDto.Message = $"Job with ID {job.Id} not found.";
				return responseDto;
			}
			var loc = await (from locations in _context.Locations where locations.IsDeleted == false && locations.Id == job.locationId select locations).FirstOrDefaultAsync();
			if (loc == null)
			{
				responseDto.IsSuccess = false;
				responseDto.Message = $"Location with Id {job.locationId} doesnt exists.";
				return responseDto;
			}

			var dept = await (from depts in _context.Departments where depts.IsDeleted == false && depts.Id == job.departmentId select depts).FirstOrDefaultAsync();
			if (dept == null)
			{
				responseDto.Message = $"Department with id {job.departmentId} does not exists!";
				responseDto.IsSuccess = false;
				return responseDto;
			}
			jb.title = job.title;
			jb.description = job.description;
			jb.closingDate = job.closingDate;
			jb.departmentId = job.departmentId;
			jb.locationId = job.locationId;
			await _context.SaveChangesAsync();
			responseDto.Message = $"Updated Job with ID {job.Id} successfully!";
			return responseDto;
		}
	}
}
