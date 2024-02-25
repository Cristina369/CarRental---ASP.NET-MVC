using CarRental.API.Models.Domain;

namespace CarRental.API.Repositories
{
	public interface ICarReservationRepository
	{
		Task<int> GetTotalReservations(Guid carId);
		Task<CarReservation> AddReservationForCar(CarReservation carReservation);

		Task<CarReservation> AddAsync(CarReservation carReservation);
		Task<IEnumerable<CarReservation>> GetReservationsByCarIdAsync(Guid carId);

	}
}
