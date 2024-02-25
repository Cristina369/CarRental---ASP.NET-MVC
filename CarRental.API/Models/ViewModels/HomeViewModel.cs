using CarRental.API.Models.Domain;

namespace CarRental.API.Models.ViewModels
{
	public class HomeViewModel
	{
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Location PickupLocation { get; set; }
        public Location ReturnLocation { get; set; }
        public IEnumerable<Car> Cars {  get; set; }
		
		public IEnumerable<Make> Makes { get; set; }

        public IEnumerable<Location> Locations { get; set; }

		//not sure about this one
		public IEnumerable<CarReservation> Reservations { get; set; }
	}
}
