using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		#region SignUp
		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (ModelState.IsValid)
			{

				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					user = new ApplicationUser
					{
						UserName = model.Email.Split('@')[0],
						Email = model.Email,
						FirstName = model.FirstName,
						LastName = model.LastName,
						IsAgree = model.IsAgree,
					};
					var Result = await _userManager.CreateAsync(user, model.Password);
					if (Result.Succeeded)
						return RedirectToAction("Login");
					else
						foreach (var error in Result.Errors)
							ModelState.AddModelError(string.Empty, error.Description);
				}
				ModelState.AddModelError(string.Empty, "Email is already exist");
				return View(model);

			}
			return View(model);
		}

		#endregion


		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid) 
			{
				var User = await _userManager.FindByEmailAsync(model.Email); 
				if (User is not null)
				{
					var Flag = await _userManager.CheckPasswordAsync(User, model.Password);
					if (Flag)
					{
						var Result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
						if (Result.Succeeded) return RedirectToAction("Index", "Home");
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Email or Password is not correct");
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email is not Exists");
				}
			}
			return View(model);

		}

		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}


		public IActionResult ForgetPassword()
		{
			return View();
		}

		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user != null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					var url = Url.Action("ResetPassword", "Account", new {email = user.Email, token = token}, Request.Scheme);
					
					var email = new Email
					{
						To = model.Email,
						Subject = "Reset Password",
						Body = url,
					};

					EmailSettings.SendEmail(email);

					return RedirectToAction(nameof(CheckYourInbox));

				}
				ModelState.AddModelError(string.Empty, "Invaild Operation, Please Try Again !!");
			}
			return View(model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}

		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"]= email;
			TempData["token"]= token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
				if(user is not null)
				{
					var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
					if(result.Succeeded)
					{
						return RedirectToAction(nameof(Login));
					}
				}
			}

			ModelState.AddModelError(string.Empty, "Invaild Operation, Please Try Again !!");

			return View(model);
		}
	}
}
