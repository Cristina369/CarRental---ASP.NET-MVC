﻿using CarRental.API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
			{
				var identityUser = new IdentityUser
				{
					UserName = registerViewModel.Username,
					Email = registerViewModel.Email
				};

				var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

				if (identityResult.Succeeded)
				{
					// assign to this user the UserRole
					var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

					if (roleIdentityResult.Succeeded)
					{
						// Show succes notification
						return RedirectToAction("Register");
					}
				}
			}
			// Show error notification
			return View();
		}

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel 
            {
                ReturnUrl = ReturnUrl 
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                if(!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return RedirectToPage(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            //Show error
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
