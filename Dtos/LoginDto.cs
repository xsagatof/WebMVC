using System.ComponentModel.DataAnnotations;

namespace WebMVC.Dtos
{
	public class LoginDto
	{
		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }
	}
}
