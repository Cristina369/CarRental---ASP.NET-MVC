using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Repositories
{
	public class CarRepository : ICarRepository
	{
		private readonly CarRentalDbContext carRentalDbContext;

		public CarRepository(CarRentalDbContext carRentalDbContext)
        {
			this.carRentalDbContext = carRentalDbContext;
		}

        public async Task<Car> AddAsync(Car car)
		{
			await carRentalDbContext.AddAsync(car);
			await carRentalDbContext.SaveChangesAsync();
			return car;
		}

		public async Task<Car?> DeleteAsync(Guid id)
		{
			var exitingCar = await carRentalDbContext.Cars.FindAsync(id);

			if(exitingCar != null)
			{
				carRentalDbContext.Cars.Remove(exitingCar);
				await carRentalDbContext.SaveChangesAsync();
				return exitingCar;
			}

			return null;
		}

		public async Task<IEnumerable<Car>> GetAllAsync()
		{
			return await carRentalDbContext.Cars.Include(c => c.Model).Include(c => c.Location).ToListAsync();
	}

		public async Task<Car?> GetAsync(Guid id)
		{
			return await carRentalDbContext.Cars
 				.Include(c => c.Model)
				.Include(c => c.Location)
				.FirstAsync(c => c.Id == id);
		}

        public async Task<List<Car>> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate, Location pickupLocation, Location returnLocation)
        {
            var cars = await carRentalDbContext.Cars.Include(c => c.Model).Include(c => c.Location).Include(c => c.Reservations).ToListAsync();
			// Get all cars (you may want to adjust this based on your data structure)

			// Example: filter cars based on availability for the specified dates and locations
			var availableCars = cars.Where(car =>
				car.Available &&
				car.Reservations.All(reservation =>
					!(pickupDate <= reservation.ReturnDate && returnDate >= reservation.PickUpDate) &&
					!(pickupLocation == reservation.ReturnLocation || returnLocation == reservation.PickUpLocation)
				)
			).ToList();


			return availableCars;
        }

		// First version
		//public async Task<List<Car>> GetAvailableCarsAsync(DateTime pickupDate, DateTime returnDate, Location pickupLocation, Location returnLocation)
		//{
		//	var cars = await carRentalDbContext.Cars.Include(c => c.Model).Include(c => c.Location).ToListAsync();
		//	// Get all cars (you may want to adjust this based on your data structure)

		//	// Example: filter cars based on availability for the specified dates and locations
		//	var availableCars = cars.Where(car =>
		//		car.Available == true &&
		//		car.Reservations.All(reservation =>
		//		reservation.ReturnDate <= pickupDate || reservation.PickUpDate >= returnDate ||
		//		reservation.ReturnLocation != pickupLocation && reservation.PickUpLocation != returnLocation)
		//	).ToList();

		//	return availableCars;
		//}

			public async Task<Car?> UpdateAsync(Car car)
		{
			var exitingCar = await carRentalDbContext.Cars
				.Include(m => m.Model)
				.Include(l => l.Location)
				.FirstOrDefaultAsync( c => c.Id == car.Id );

			if (exitingCar != null)
			{
				exitingCar.Id = car.Id;
				exitingCar.Price = car.Price;
				exitingCar.Year = car.Year;
				exitingCar.Type = car.Type;
				exitingCar.Description = car.Description;
				exitingCar.ImageCar = car.ImageCar;
				exitingCar.Available = car.Available;
				exitingCar.ModelId = car.ModelId;
				exitingCar.LocationId = car.LocationId;

				await carRentalDbContext.SaveChangesAsync();
				await carRentalDbContext.SaveChangesAsync();
				return exitingCar;
			}
			return null;
		}
	}
}
