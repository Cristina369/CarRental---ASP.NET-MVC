using CarRental.API.Models.Domain;
using CarRental.API.Models.ViewModels;
using CarRental.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRental.API.Controllers
{
	public class CarReservationController : Controller
	{
		private readonly ICarReservationRepository carReservationRepository;
		private readonly ILocationRepository locationRepository;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		public CarReservationController(ICarReservationRepository carReservationRepository,
			ILocationRepository locationRepository,
			SignInManager<IdentityUser> signInManager,
			UserManager<IdentityUser> userManager)
        {
			this.carReservationRepository = carReservationRepository;
			this.locationRepository = locationRepository;
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

		[HttpGet]
        public async Task<IActionResult> Add()
		{
			var locations = await locationRepository.GetAllAsync();

			var location = new AddReservationRequest
			{
				Locations = locations.Select(l => new SelectListItem
				{
					Text = l.Name,
					Value = l.Id.ToString()
				})
			};

			return View(location);
		}

		[HttpPost]
		public async Task<IActionResult> Add(CarReservationViewModel carReservationViewModel)
		{
			var carReservationDM = await locationRepository.GetAllAsync();
			
				var carReservation = new CarReservation
				{
					PickUpDate = carReservationViewModel.PickUpDate,
					ReturnDate = carReservationViewModel.ReturnDate,
					UserId = Guid.Parse(userManager.GetUserId(User)),
					CarId = carReservationViewModel.Id,
					DateReservation = DateTime.Now,
					Details = carReservationViewModel.Details
				};

			//var selectedlocationpickupid = guid.parse(carreservationviewmodel.pickupdate);

			//await carreservationrepository.addasync(domainmodel);
			//return redirecttoaction("index", "cars");

			return View();
		}
	}
}
