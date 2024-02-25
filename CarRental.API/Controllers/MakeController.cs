using CarRental.API.Models.Domain;
using CarRental.API.Models.ViewModels;
using CarRental.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
	//[Authorize(Roles = "Admin")]
	public class MakeController : Controller
	{
		private readonly IMakeRepository makeRepository;

		public MakeController(IMakeRepository makeRepository)
        {
			this.makeRepository = makeRepository;
		}

		[HttpGet]
        public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Add")]
		public async Task<IActionResult> Add([FromBody] AddMakeRequest addMakeRequest)
		{
			var make = new Make
			{
				Name = addMakeRequest.Name
			};

			await makeRepository.AddAsync(make);

			var response = new AddMakeRequest
			{
				Id = make.Id,
				Name = make.Name
			};


			return Ok(response);// return View();
		}

		[HttpGet]
		[ActionName("List")]
		public async Task<IActionResult> List()
		{
			var makes = await makeRepository.GetAllAsync();

            var response = new List<AddMakeRequest>();
            foreach (var make in makes)
            {
                response.Add(new AddMakeRequest
                {
                    Id = make.Id,
                    Name = make.Name
                });
            }
            return Ok(response);// return View(makes);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid id)
		{
            var make = await makeRepository.DeleteAsync(id);

            if (make is null)
            {
                return NotFound();
            };

            // Convert to Dto before return results
            var response = new AddMakeRequest
            {
                Id = make.Id,
                Name = make.Name
            };

			return Ok(response);

            //var make = await makeRepository.GetAsync(id);

            //if(make != null)
            //{
            //	var deletedMake = await makeRepository.DeleteAsync(make.Id);
            //	return RedirectToAction("List");
            //}
            //return View("List");
        }
	}
}
