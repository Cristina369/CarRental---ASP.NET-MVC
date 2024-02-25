using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRental.API.Models.ViewModels
{
	public class EditCarRequest
	{
		public Guid Id { get; set; }
		public double Price { get; set; }
		public int Year { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public string ImageCar { get; set; }
		public bool Available { get; set; }
		// public Guid LocationId { get; set; }
		public IEnumerable<SelectListItem> Models { get; set; }
		public string SelectedModel { get; set; }

		public IEnumerable<SelectListItem> Locations { get; set; }
		public string SelectedLocation { get; set; }
	}
}
