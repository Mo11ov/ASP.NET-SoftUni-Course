using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist.Contracts;
using Watchlist.Models;

namespace Watchlist.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;


        public MoviesController(IMovieService movieService)
        {
            this.movieService = movieService;        
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddMovieViewModel()
            {
                Genres = await movieService.GetGenresAsync()
            };
            
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await movieService.AddMovieAsync(model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");

                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var allMovies = await movieService.GetMoviesAsync();

            return View(allMovies);
        }

        public async Task<IActionResult> AddToCollection(int movieId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await movieService.AddToUserWatchedMoviesAsync(movieId, userId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Watched()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var watchedMovies = await movieService.GetUserWatchedMovies(userId);

            return View(watchedMovies);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int movieId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await movieService.RemoveFromUserCollection(movieId, userId);

            return RedirectToAction(nameof(Watched));
        }
    }
}
