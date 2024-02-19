using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using JobOpeningsApiWebService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningsApiWebService.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/departments")]
	public class DepartmentsController : ControllerBase
	{
		private readonly IDepartment _departmentRepo;
		public DepartmentsController(IDepartment departmentRepo)
		{
			_departmentRepo = departmentRepo;
		}

		// GET: api/<DepartmentsController>
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Get()
		{
			var depts = await _departmentRepo.GetDepartments();
			return Ok(depts);
		}

		// GET api/<DepartmentsController>/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> Get(int id)
		{
			var dept = await _departmentRepo.GetDepartmentById(id);
			if (dept == null)
			{
				return NotFound($"Department with Id {id} not found");
			}
			return Ok(dept);
		}

		// POST api/<DepartmentsController>
		[HttpPost]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Post([FromBody] DepartmentReqDto department)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			Department dept = new()
			{
				title = department.title
			};
			ResponseDto res = await _departmentRepo.AddDepartment(dept);
			if (res.IsSuccess)
			{
				return StatusCode(StatusCodes.Status201Created, res.Message);
			}
			return BadRequest("Failed to create the department");
		}

		// PUT api/<DepartmentsController>/5
		[HttpPut("{id}")]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Put(int id, [FromBody] Department department)
		{
			if (id != department.Id)
			{
				return BadRequest("Path Id and departmet Id does not match");
			}
			ResponseDto res = await _departmentRepo.UpdateDepartment(department);
			if (res.IsSuccess)
			{
				return Ok(res.Message);
			}
			return BadRequest(res.Message);
		}

		// DELETE api/<DepartmentsController>/5
		[HttpDelete("{id}")]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Delete(int id)
		{
			ResponseDto res = await _departmentRepo.DeleteDepartment(id);
			if (!res.IsSuccess)
			{
				return BadRequest(res.Message);
			}
			return Ok(res.Message);
		}
	}
}
