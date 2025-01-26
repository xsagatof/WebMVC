using System.ComponentModel.DataAnnotations;

namespace WebMVC.Dtos
{
	public class RegisterDto
	{
		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Email is required")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }
	}
}
