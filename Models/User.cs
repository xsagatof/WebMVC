using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVC.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		
		[Required(ErrorMessage = "Username is required")]
		[Column(TypeName = "nvarchar(100)")]
		public string Username { get; set; }
		
		[Required(ErrorMessage = "Email is required")]
		[Column(TypeName = "nvarchar(100)")]
		public string Password { get; set; }
		
		[Required(ErrorMessage = "Email is required")]
		[Column(TypeName = "nvarchar(100)")]
		public string Email { get; set; }
	}
}
