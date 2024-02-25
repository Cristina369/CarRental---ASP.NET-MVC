using CarRental.API.Models.Domain;

namespace CarRental.API.Models.ViewModels
{
	public class DeleteMakeRequest
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public ICollection<Model> Models { get; set; } // one make can have multiples models

	}
}
