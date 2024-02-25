using CarRental.API.Models;
using CarRental.API.Models.ViewModels;
using CarRental.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace CarRental.API.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository carRepository;
		private readonly IMakeRepository makeRepository;

		public HomeController(ILogger<HomeController> logger,
			ICarRepository carRepository,
			IMakeRepository makeRepository)
		{
			_logger = logger;
            this.carRepository = carRepository;
			this.makeRepository = makeRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var cars = await carRepository.GetAllAsync();
			var makes = await makeRepository.GetAllAsync();

			var model = new HomeViewModel
			{
				Cars = cars,
				Makes = makes
			};

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [HttpGet]
        public async Task<IActionResult> GetMatchingLocations(string locationSearch)
        {
            var cars = await carRepository.GetAllAsync();
            var matchingLocations = cars
                .Select(car => car.Location.Name)
                .Distinct()
                .Where(location => location.StartsWith(locationSearch, StringComparison.OrdinalIgnoreCase))
                .ToList();


            //var matchingLocations = cars
            //    .Select(car => new
            //    {
            //        Id = car.Id,
            //        ModelName = car.Model.Name
            //    })
            //    .Distinct()
            //    .Where(location => location.ModelName.StartsWith(locationSearch, StringComparison.OrdinalIgnoreCase))
            //    .ToList();
            //Console.WriteLine("Matching Locations are", matchingLocations);
            //return Json(matchingLocations);


            return Json(matchingLocations);
        }


        //[HttpGet]
        //public async Task<IActionResult> Search()
        //{
        //    var cars = await carRepository.GetAllAsync();
        //    var makes = await makeRepository.GetAllAsync();

        //    var model = new HomeViewModel
        //    {
        //        Cars = cars,
        //        Makes = makes
        //    };

        //    return View(model);
        //}

        [HttpGet]
        public async Task<IActionResult> Search(HomeViewModel homeViewModel)
        {
            // Your search logic here
            // You can access search parameters like searchViewModel.PickupDate, searchViewModel.ReturnDate, etc.

            // Perform the search using the parameters and retrieve the matching cars
            var availableCars = await carRepository.GetAvailableCarsAsync(homeViewModel.PickupDate, homeViewModel.ReturnDate, homeViewModel.PickupLocation, homeViewModel.ReturnLocation);

            //var matchingCars = await carRepository.SearchAsync(searchViewModel.PickupDate, searchViewModel.ReturnDate, searchViewModel.PickupLocation, searchViewModel.ReturnLocation);

            // Update the model with the matching cars
            var model = new HomeViewModel
            {
                Cars = availableCars
                // Populate other model properties as needed
            };

            return View("Search", model);
        }

    }
}