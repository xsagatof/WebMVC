using System.ComponentModel.DataAnnotations.Schema;

namespace WebMVC.Models
{
	public class Image
	{
		public int Id { get; set; }
		public string Url { get; set; }
		public string Type { get; set; } //poster or fragments from movie
		
	}
}
