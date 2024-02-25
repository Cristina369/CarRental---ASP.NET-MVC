using CarRental.API.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRental.API.Models.ViewModels
{
	public class AddModelRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<SelectListItem> Makes { get; set; }
		public string SelectedMake { get; set; }

	}
}
