using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Link { get; set; }
}
