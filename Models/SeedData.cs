using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebMVC.Data;
using System;
using System.Linq;
using WebMVC.Models;

namespace WebMVC.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new WebMVCContext(
                   serviceProvider.GetRequiredService<
                       DbContextOptions<WebMVCContext>>()))
        {
            // Look for any movies.
            if (context.Movie.Any())
            {
                return;   // DB has been seeded
            }
            context.Movie.AddRange(
                new Movie
                {
                    Title = "Wrath of Man",
                    Genre = "Action",
                    Link = "https://www.imdb.com/title/tt11083552"
                },
                new Movie
                {
                    Title = "Oppenheimer",
                    Genre = "Documentary", 
                    Link = "https://www.imdb.com/title/tt15398776"
                },        
                new Movie
                {
                    Title = "The Equalizer",
                    Genre = "Action",
                    Link = "https://www.imdb.com/title/tt0455944"
                },
                new Movie
                {
                    Title = "Le Mans '66 (Ford v Ferrari)",
                    Genre = "Documentary",
                    Link = "https://www.imdb.com/title/tt1950186"
                }

            );
            context.SaveChanges();
        }
    }
}
