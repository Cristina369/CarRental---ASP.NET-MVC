using CarRental.API.Models.Domain;
using CarRental.API.Models.ViewModels;
using CarRental.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
	public class LocationController : Controller
	{
		private readonly ILocationRepository locationRepository;

		public LocationController(ILocationRepository locationRepository)
        {
			this.locationRepository = locationRepository;
		}

		[HttpGet]
        public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Add")]
		public async Task<IActionResult> Add(AddLocationRequest addLocationRequest)
		{
			var location = new Location
			{
				Name = addLocationRequest.Name,
				City = addLocationRequest.City,
				Street = addLocationRequest.Street,
				Phone = addLocationRequest.Phone
			};

			await locationRepository.AddAsync(location);

			return RedirectToAction("Add");
		}

		[HttpGet]
		public async Task<IActionResult> List()
		{
			var locations = await locationRepository.GetAllAsync();
			return View(locations);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var location = await locationRepository.GetAsync(id);
			
			if(location != null)
			{
				var editLocationRequest = new EditLocationRequest
				{
					Id = location.Id,
					Name = location.Name,
					City = location.City,
					Street = location.Street,
					Phone = location.Phone
				};
				return View(editLocationRequest);
			}
			return View(null);
		}


		[HttpPost]
		public async Task<IActionResult> Edit(EditLocationRequest editLocationRequest)
		{
			var location = new Location
			{
				Id = editLocationRequest.Id,
				Name = editLocationRequest.Name,
				City = editLocationRequest.City,
				Street = editLocationRequest.Street,
				Phone = editLocationRequest.Phone
			};

			var updatedLocation = await locationRepository.UpdateAsync(location);
			
			if(updatedLocation != null)
			{
				return RedirectToAction("List");
			}
			return RedirectToAction("Edit");
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid id)
		{
			var location = await locationRepository.GetAsync(id);

			if(location != null)
			{
				var deletedLocation = await locationRepository.DeleteAsync(location.Id);
				return RedirectToAction("List");
			}
			return View("List");
		}

	}
}
