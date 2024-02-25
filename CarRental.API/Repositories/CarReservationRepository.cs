using CarRental.API.Data;
using CarRental.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Repositories
{
	public class CarReservationRepository : ICarReservationRepository
	{
		private readonly CarRentalDbContext carRentalDbContext;

		public CarReservationRepository(CarRentalDbContext carRentalDbContext)
        {
			this.carRentalDbContext = carRentalDbContext;
		}

		public async Task<CarReservation> AddAsync(CarReservation carReservation)
		{
			await carRentalDbContext.Reservations.AddAsync(carReservation);
			await carRentalDbContext.SaveChangesAsync();
			return carReservation;
		}

		public async Task<CarReservation> AddReservationForCar(CarReservation carReservation)
		{
			await carRentalDbContext.Reservations.AddAsync(carReservation);
			await carRentalDbContext.SaveChangesAsync();
			return carReservation;
		}

		public async Task<IEnumerable<CarReservation>> GetReservationsByCarIdAsync(Guid carId)
		{
			return await carRentalDbContext.Reservations
				.Where(r => r.CarId == carId).ToListAsync();
		}

		public async Task<int> GetTotalReservations(Guid carId)
		{
			return await carRentalDbContext.Reservations.CountAsync( r => r.CarId == carId );
		}
	}
}
