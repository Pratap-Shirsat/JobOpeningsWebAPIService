using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using JobOpeningsApiWebService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningsApiWebService.Controllers
{
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/locations")]
	[ApiController]
	public class LocationsController : ControllerBase
	{
		private ILocation _locationRepository;

		public LocationsController(ILocation locationRepository)
		{
			_locationRepository = locationRepository;
		}

		// GET: api/<LocationsController>
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Get()
		{
			return Ok(await _locationRepository.GetLocations());
		}

		// GET api/<LocationsController>/5
		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> Get(int id)
		{
			var location = await _locationRepository.GetLocationById(id);
			if (location == null)
			{
				return NotFound($"Location with id {id} not found");
			}
			return Ok(location);
		}

		// POST api/<LocationsController>
		[HttpPost]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> AddLocation([FromBody] LocationReqDto location)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				Location loc = new();
				loc.city = location.city;
				loc.country = location.country;
				loc.title = location.title;
				loc.zip = location.zip;
				loc.state = location.state;

				bool isSuccess = await _locationRepository.AddLocation(loc);
				if (!isSuccess)
				{
					return StatusCode(StatusCodes.Status400BadRequest);
				}
				return StatusCode(StatusCodes.Status201Created, "Created location successfully.");
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return BadRequest(message);
			}
		}

		// PUT api/<LocationsController>/5
		[HttpPut("{id}")]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Put(int id, [FromBody] Location location)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (id != location.Id)
			{
				return BadRequest("Id is not valid or is missmatching");
			}
			var res = await _locationRepository.UpdateLocation(location);
			if (!res.IsSuccess)
			{
				return NotFound(res.Message);
			}
			return Ok(res.Message);
		}

		// DELETE api/<LocationsController>/5
		[HttpDelete("{id}")]
		[Authorize("AdminPolicy")]
		public async Task<IActionResult> Delete(int id)
		{
			var res = await _locationRepository.DeleteLocation(id);
			if (!res.IsSuccess)
			{
				return NotFound(res.Message);
			}
			return Ok(res.Message);
		}
	}
}
