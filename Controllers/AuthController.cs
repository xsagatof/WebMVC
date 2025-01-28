using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebMVC.Data;
using WebMVC.Dtos;
using WebMVC.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebMVC.Controllers
{
	public class AuthController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;

		public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto registerDto)
		{
			var userExists = await _userManager.FindByNameAsync(registerDto.Username);

			if (userExists != null)
			{
				return new ContentResult
				{
					Content = "User already exists!",
					ContentType = "text/plain",
					StatusCode = 500
				};
			}

			User user = new User()
			{
				UserName = registerDto.Username,
				Email = registerDto.Email
			};

			var result = await _userManager.CreateAsync(user, registerDto.Password);

			if (!result.Succeeded)
			{
				return new ContentResult()
				{
					Content = "User creation failed",
					ContentType = "text/plain",
					StatusCode = 500
				};
			}

			return RedirectToAction("Login", "Auth");
		}

		public IActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			var user = await _userManager.FindByNameAsync(loginDto.Username);

			if (user != null && await  _userManager.CheckPasswordAsync(user, loginDto.Password));
			{
				await _signInManager.SignInAsync(user, isPersistent: false);

				return RedirectToAction("Index", "Movies");
			}

			ViewBag.ErrorMessage = "Invalid username or password";
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Movies");
		}

	}
}
