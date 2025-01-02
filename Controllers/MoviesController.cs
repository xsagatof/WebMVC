using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebMVC.Data;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly WebMVCContext _context; // DI

        public MoviesController(WebMVCContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
	        if (_context.Movie == null)
	        {
		        return Problem("Entity set 'WebMVCContext.Movie' is null.");
	        }

	        IQueryable<string> genreQuery = from m in _context.Movie orderby m.Genre select m.Genre;

            var movies = from m in _context.Movie select m;

	        if (!string.IsNullOrEmpty(searchString))
	        {
		        movies = movies.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
	        }

	        if (!string.IsNullOrEmpty(movieGenre))
	        {
		        movies = movies.Where(x => x.Genre == movieGenre);
	        }

	        var movieGenreVM = new MovieGenreViewModel
	        {
		        Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
		        Movies = await movies.ToListAsync(),
                MovieGenre = movieGenre,
                SearchString = searchString
	        };

			return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Link,ImagePoster")] Movie movie, IFormFile imagePoster)
        {
            if (ModelState.IsValid)
            {
	            if (imagePoster != null)
	            {
		            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagePoster.FileName); // Generate a unique file name

					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName); // Define the path to save the image

					// Save the file to the server
					using (var stream = new FileStream(filePath, FileMode.Create))
		            {
			            await imagePoster.CopyToAsync(stream);
		            }

		            movie.ImagePoster = "/images/" + fileName; // Save the relative path in the database
				}

				_context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

		// POST: Movies/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,Link,ImagePoster")] Movie movie, IFormFile imagePoster)
		{
			if (!MovieExists(movie.Id))
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Fetch the existing movie from the database
					var existingMovie = await _context.Movie.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
					if (existingMovie == null)
					{
						return NotFound();
					}

					// Check if a new image is uploaded
					if (imagePoster != null)
					{
						// Generate a unique file name
						var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagePoster.FileName);

						// Define the path to save the new image
						var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

						// Save the new image to the server
						await using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await imagePoster.CopyToAsync(stream);
						}

						// Remove the old image file from the server
						if (!string.IsNullOrEmpty(existingMovie.ImagePoster))
						{
							var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingMovie.ImagePoster.TrimStart('/'));
							if (System.IO.File.Exists(oldFilePath))
							{
								System.IO.File.Delete(oldFilePath);
							}
						}

						// Update the image path in the database entity
						movie.ImagePoster = "/images/" + fileName;
					}
					else
					{
						// Retain the existing image if no new image is uploaded
						movie.ImagePoster = existingMovie.ImagePoster;
					}

					// Update the movie in the database
					_context.Update(movie);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!MovieExists(movie.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(movie);
		}


		// GET: Movies/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetPoster(int id)
        {
	        var movie = _context.Movie.Find(id);
	        if (movie == null || movie.ImagePoster == null)
	        {
		        return NotFound(); // Handle missing images
	        }
	        return File(movie.ImagePoster, "image/jpeg"); // Adjust MIME type if needed
        }

		private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
