using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.ViewModels;
using CarRental.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRental.API.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CarController : Controller
	{
		private readonly IModelRepository modelRepository;
		private readonly ILocationRepository locationRepository;
		private readonly ICarRepository carRepository;

		public CarController(ICarRepository carRepository, IModelRepository modelRepository, ILocationRepository locationRepository)
        {
			this.modelRepository = modelRepository;
			this.locationRepository = locationRepository;
			this.carRepository = carRepository;
		}

        [HttpGet]
		public async Task<IActionResult> Add()
		{
			var models = await modelRepository.GetAllAsync();
			var locations = await locationRepository.GetAllAsync();

			var model = new AddCarRequest
			{
				Models = models.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString()
				}),
				Locations = locations.Select( l => new SelectListItem
				{
					Text = l.Name,
					Value = l.Id.ToString()
				})
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddCarRequest addCarRequest)
		{
			var carModelDM = await modelRepository.GetAllAsync();

			var car = new Car
			{
				Price = addCarRequest.Price,
				Year = addCarRequest.Year,
				Type = addCarRequest.Type,
				Description = addCarRequest.Description,
				ImageCar = addCarRequest.ImageCar, 
				Available = addCarRequest.Available
			}; 

			
			var selectedModelIdGuid = Guid.Parse(addCarRequest.SelectedModel);// ToString()
			var existingModel = await modelRepository.GetAsync(selectedModelIdGuid);

			if (existingModel != null)
			{
				var selectedModel = new Model
				{ 
					Id = existingModel.Id
				};
				car.ModelId = selectedModel.Id;
			}

			var selectedLocationIdGuid = Guid.Parse(addCarRequest.SelectedLocation);// ToString()
			var existingLocation = await locationRepository.GetAsync(selectedLocationIdGuid);

			if (existingLocation != null)
			{
				var selectedLocation = new Location
				{
					Id = existingLocation.Id
				};
				car.LocationId = selectedLocation.Id;
			}

			await carRepository.AddAsync(car);
			return RedirectToAction("Add");
			
		}


		[HttpGet]
		public async Task<IActionResult> List()
		{
			var cars = await carRepository.GetAllAsync();

			return View(cars);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var car = await carRepository.GetAsync(id);
			var modelDM = await modelRepository.GetAllAsync();
			var locationDM = await locationRepository.GetAllAsync();

			if(car != null)
			{
				var carModel = new EditCarRequest
				{
					Id = car.Id,
					Price = car.Price,
					Year = car.Year,
					Type = car.Type,
					Description = car.Description,
					ImageCar = car.ImageCar,
					Available = car.Available,
					Models = modelDM.Select(c => new SelectListItem
					{
						Text = c.Name,
						Value = c.Id.ToString()
					}),
					SelectedModel = car.ModelId.ToString(),
					Locations = locationDM.Select(l => new SelectListItem
					{
						Text = l.Name,
						Value = l.Id.ToString()
					}),
					SelectedLocation = car.LocationId.ToString()
				};
				return View(carModel);
			}

			return View(null);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditCarRequest editCarRequest)
		{

			var carModelDM = new Car
			{
				Id = editCarRequest.Id,
				Price = editCarRequest.Price,
				Year = editCarRequest.Year,
				Type = editCarRequest.Type,
				Description = editCarRequest.Description,
				ImageCar = editCarRequest.ImageCar,
				Available = editCarRequest.Available,
			};


			var selectedModelIdGuid = Guid.Parse(editCarRequest.SelectedModel);// ToString()
			var existingModel = await modelRepository.GetAsync(selectedModelIdGuid);

			if (existingModel != null)
			{
				var selectedModel = new Model
				{
					Id = existingModel.Id
				};
				carModelDM.ModelId = selectedModel.Id;
			}

			var selectedLocationIdGuid = Guid.Parse(editCarRequest.SelectedLocation);// ToString()
			var existingLocation = await locationRepository.GetAsync(selectedLocationIdGuid);

			if (existingLocation != null)
			{
				var selectedLocation = new Location
				{
					Id = existingLocation.Id
				};
				carModelDM.LocationId = selectedLocation.Id;
			}

			var updatedCar = await carRepository.UpdateAsync(carModelDM);

			if(updatedCar != null)
			{
				return RedirectToAction("Edit");
			}

			return RedirectToAction("Edit");

		}

		[HttpPost]
		public async Task<IActionResult> Delete(EditCarRequest editCarRequest)
		{
			var deletedCar = await carRepository.DeleteAsync(editCarRequest.Id);

			if(deletedCar != null)
			{
				return RedirectToAction("List");
			}
			return RedirectToAction("Edit", new { id = editCarRequest.Id });
		}

	}
}

