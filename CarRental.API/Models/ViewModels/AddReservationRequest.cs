using CarRental.API.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRental.API.Models.ViewModels
{
	public class AddReservationRequest
	{
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateReservation { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Details { get; set; }
		public IEnumerable<SelectListItem> Locations { get; set; }
		public string PickUpLocation { get; set; }
        public string ReturnLocation { get; set; }
	}
}
