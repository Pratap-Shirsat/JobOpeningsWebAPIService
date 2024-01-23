using JobOpeningsApiWebService.Data;
using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningsApiWebService.Services
{
	public class LocationRepository : ILocation
	{
		private JobOpeningsDbContext _dbContext;

		public LocationRepository(JobOpeningsDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> AddLocation(Location location)
		{
			await _dbContext.Locations.AddAsync(location);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<ResponseDto> DeleteLocation(int Id)
		{
			ResponseDto res = new();
			var location = await (from locations in _dbContext.Locations where locations.Id == Id && locations.IsDeleted == false select locations).FirstOrDefaultAsync();
			if (location == null)
			{
				res.IsSuccess = false;
				res.Message = $"Location with ID {Id} doesnt exists.";
				return res;
			}
			location.IsDeleted = true;
			await _dbContext.SaveChangesAsync();

			res.Message = $"Location with ID {Id} deleted successfully.";
			return res;
		}

		public async Task<Location?> GetLocationById(int Id)
		{
			var location = await (from locations in _dbContext.Locations.Where(x => x.Id == Id && x.IsDeleted == false) select locations).FirstOrDefaultAsync();
			return location;
		}

		public async Task<IEnumerable<Location>> GetLocations()
		{
			return await _dbContext.Locations.Where(x => x.IsDeleted == false).ToListAsync();
		}

		public async Task<ResponseDto> UpdateLocation(Location location)
		{
			ResponseDto res = new();
			var loc = await _dbContext.Locations.Where(x => x.IsDeleted == false && x.Id == location.Id).FirstOrDefaultAsync();
			if (loc == null)
			{
				res.IsSuccess = false;
				res.Message = $"Location with ID {location.Id} not found.";
				return res;
			}
			loc.title = location.title;
			loc.country = location.country;
			loc.city = location.city;
			loc.state = location.state;
			loc.zip = location.zip;

			await _dbContext.SaveChangesAsync();
			res.Message = $"Updated location with ID {location.Id} successfully.";
			return res;
		}
	}
}
