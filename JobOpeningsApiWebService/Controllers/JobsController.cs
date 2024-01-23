using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using JobOpeningsApiWebService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningsApiWebService.Controllers
{
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/jobs")]
	[ApiController]
	public class JobsController : ControllerBase
	{
		private IJobs _jobRepo;

		public JobsController(IJobs jobRepo)
		{
			_jobRepo = jobRepo;
		}

		// GET: api/<JobsController>
		[HttpGet("[action]")]
		[Authorize]
		public async Task<IActionResult> list([FromQuery] jobListReqDto jobReq)
		{
			var jobs = await _jobRepo.GetJobs(jobReq.q, jobReq?.locationId, jobReq?.departmentId);
			var filteredJobs = jobs.Skip((jobReq.pageNo - 1) * jobReq.pageSize).Take(jobReq.pageSize);
			return Ok(new { total = jobs.Count(), data = filteredJobs });
		}

		// GET api/<JobsController>/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> Get(int id)
		{
			var job = await _jobRepo.GetJobById(id);
			if (job == null)
			{
				return NotFound($"Job with id {id} not found");
			}
			return Ok(job);
		}

		// POST api/<JobsController>
		[HttpPost]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Post([FromBody] JobRequestDto job)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			Job jb = new();
			jb.title = job.title;
			jb.description = job.description;
			jb.locationId = job.locationId;
			jb.departmentId = job.departmentId;
			jb.closingDate = job.closingDate;
			jb.code = generateJobCode();
			var res = await _jobRepo.AddJob(jb);
			if (res.IsSuccess)
			{
				return StatusCode(StatusCodes.Status201Created, res.Message);
			}
			return BadRequest(res.Message);
		}

		// PUT api/<JobsController>/5
		[HttpPut("{id}")]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Put(int id, [FromBody] Job job)
		{
			if (id != job.Id)
			{
				return BadRequest("Path id and job id does not match!");
			}
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var res = await _jobRepo.UpdateJob(job);
			if (!res.IsSuccess)
			{
				return BadRequest(res.Message);
			}
			return Ok(res.Message);
		}

		// DELETE api/<JobsController>/5
		[HttpDelete("{id}")]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _jobRepo.DeleteJob(id);
			if (!res.IsSuccess)
			{
				return BadRequest(res.Message);
			}
			return Ok(res.Message);
		}

		private string generateJobCode()
		{
			Random random = new Random();
			string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			char[] code = new char[6];

			for (int i = 0; i < 6; i++)
			{
				code[i] = allowedChars[random.Next(0, allowedChars.Length)];
			}

			return new string(code);
		}
	}
}
