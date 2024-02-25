using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.ViewModels;
using CarRental.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Controllers
{
	//[Authorize(Roles = "Admin")]
	public class ModelController : Controller
	{
		private readonly IModelRepository modelRepository;
		private readonly IMakeRepository makeRepository;

		public ModelController(IModelRepository modelRepository, IMakeRepository makeRepository)
        {
			this.modelRepository = modelRepository;
			this.makeRepository = makeRepository;
		}

        [HttpGet]
		public async Task<IActionResult> Add()
		{
			var makes = await makeRepository.GetAllAsync();

			var make = new AddModelRequest
			{
				Makes = makes.Select(x => new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString()
				})
			};

			return View(make);
		}

		[HttpPost]
		public async Task<IActionResult> Add( AddModelRequest addModelRequest)
		{
			var modelMakeDM = await makeRepository.GetAllAsync();

			var model = new Model
			{
				Name = addModelRequest.Name,
				Description = addModelRequest.Description
			};

			var selectedMakeIdGuid = Guid.Parse(addModelRequest.SelectedMake);
			var existingMake = await makeRepository.GetAsync(selectedMakeIdGuid);

			if(existingMake != null)
			{
				var selectedMake = new Make
				{
					Id = existingMake.Id
				};
				model.MakeId = selectedMake.Id;
			}

			await modelRepository.AddAsync(model);

			return RedirectToAction("List");
		}

		[HttpGet]
		[ActionName("List")]
		public async Task<IActionResult> List()
		{
			var models = await modelRepository.GetAllAsync();
            var makeDM = await makeRepository.GetAllAsync();


            var response = new List<ModelRequest>();
            foreach (var model in models)
            {
                var make = makeDM.FirstOrDefault(m => m.Id == model.MakeId);
                var makeName = make != null ? make.Name : "Unknown";

                response.Add(new ModelRequest
                {
                    Id = model.Id,
                    Name = model.Name,
					Description = model.Description,
                    MakeId = makeName
                });
            }
            return Ok(response);// return View(models);
        }

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var model = await modelRepository.GetAsync(id);
			var makeDM = await makeRepository.GetAllAsync();

			if (model != null)
			{
				var editModelRequest = new EditModelRequest 
				{
					Id = model.Id ,
					Name = model.Name ,
					Description = model.Description,
					Makes = makeDM.Select( m => new SelectListItem
					{
						Text = m.Name,
						Value = m.Id.ToString() 
					}),
					SelectedMake = model.MakeId.ToString()
				};
				return View(editModelRequest);
			}
			return View(null);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditModelRequest editModelRequest)
		{
			var model = new Model
			{
				Id = editModelRequest.Id ,
				Name = editModelRequest.Name,
				Description = editModelRequest.Description
			};

			var selectedMakeIdGuid = Guid.Parse(editModelRequest.SelectedMake);
			var existingMake = await makeRepository.GetAsync(selectedMakeIdGuid);

			if(existingMake != null)
			{
				var selectedMake = new Make
				{
					Id = existingMake.Id
				};
				model.MakeId = selectedMake.Id;
			}
			var updatedModel = await modelRepository.UpdateAsync(model);

			if(updatedModel != null)
			{
				return RedirectToAction("List");
			}
			return RedirectToAction("Edit");
		}

		[HttpPost] 
		public async Task<IActionResult> Delete(EditModelRequest editModelRequest)
		{
			var deletedModel = await modelRepository.DeleteAsync(editModelRequest.Id);

			if(deletedModel != null)
			{
				//Show succes notification
				return RedirectToAction("List");
			}

				// Show error notification
			return RedirectToAction("Edit", new { id = editModelRequest.Id });
		}
	}
}
