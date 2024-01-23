using JobOpeningsApiWebService.Dto;
using JobOpeningsApiWebService.Models;

namespace JobOpeningsApiWebService.Services
{
	public interface ILocation
	{
		Task<IEnumerable<Location>> GetLocations();
		Task<Location?> GetLocationById(int Id);
		Task<bool> AddLocation(Location location);
		Task<ResponseDto> DeleteLocation(int Id);
		Task<ResponseDto> UpdateLocation(Location location);
	}
}
