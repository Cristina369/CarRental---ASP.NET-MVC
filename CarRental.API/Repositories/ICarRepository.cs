using CarRental.API.Models.Domain;

namespace CarRental.API.Repositories
{
	public interface ICarRepository
	{
		Task<IEnumerable<Car>> GetAllAsync();
		Task<Car?> GetAsync(Guid id);
		Task<Car> AddAsync(Car car);
		Task<Car?> UpdateAsync(Car car);
		Task<Car?> DeleteAsync(Guid id);
        Task<List<Car>> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate, Location pickupLocation, Location returnLocation);

    }
}
