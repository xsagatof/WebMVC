using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models;

public class Movie
{
    public int Id { get; set; }

    [StringLength(60, MinimumLength = 2)]
    [Required]
    public string? Title { get; set; }

    [RegularExpression(@"^[A-z]+[a-zA-z\s]*$")]
    [Required]
    [StringLength(30)]
    public string? Genre { get; set; }
    public string? Link { get; set; }
    public string? ImagePoster { get; set; }
}
