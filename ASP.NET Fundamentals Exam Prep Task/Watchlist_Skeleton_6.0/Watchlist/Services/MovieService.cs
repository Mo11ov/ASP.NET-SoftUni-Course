using Microsoft.EntityFrameworkCore;
using Watchlist.Contracts;
using Watchlist.Data;
using Watchlist.Data.Entities;
using Watchlist.Models;

namespace Watchlist.Services
{
    public class MovieService : IMovieService
    {
        private readonly WatchlistDbContext context;

        public MovieService(WatchlistDbContext context)
        {
            this.context = context;
        }

        public async Task AddMovieAsync(AddMovieViewModel model)
        {
            var movie = new Movie()
            {
                Title = model.Title,
                Director = model.Director,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                GenreId = model.GenreId,
            };

            await context.Movies.AddAsync(movie);
            await context.SaveChangesAsync();
        }

        public async Task AddToUserWatchedMoviesAsync(int movieId, string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UsersMovies)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user");
            }

            var movie = await context.Movies.FindAsync(movieId);

            if (movie  == null)
            {
                throw new ArgumentException("Invalid movie");
            }

            if (!user.UsersMovies.Any(x => x.MovieId == movieId))
            {
                user.UsersMovies.Add(new UserMovie
                {
                    MovieId = movieId,
                    UserId = userId,
                    User = user,
                    Movie = movie
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Genre>> GetGenresAsync()
        {
            var genres = await context.Genres.ToListAsync();
            
            return genres;
        }

        public async Task<ICollection<MoviesViewModel>> GetMoviesAsync()
        {
            var movies = await context.Movies.Include(m => m.Genre).ToListAsync();

            var allMovies = movies.Select(x => new MoviesViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Director = x.Director,
                Rating = x.Rating,
                ImageUrl = x.ImageUrl,
                Genre = x.Genre.Name
            }).ToList();

            return allMovies;
        }

        public async Task<ICollection<MoviesViewModel>> GetUserWatchedMovies(string userId)
        {
            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(u => u.UsersMovies)
                .ThenInclude(um => um.Movie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user");
            }

            var whatchedMovies = user.UsersMovies.Select(x => new MoviesViewModel
            {
                Title = x.Movie.Title,
                Director = x.Movie.Director,
                Genre = x.Movie.Genre.Name,
                Rating = x.Movie.Rating,
                ImageUrl =x.Movie.ImageUrl,
                Id = x.MovieId
            }).ToList();

            return whatchedMovies;
        }

        public async Task RemoveFromUserCollection(int movieId, string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UsersMovies)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user");
            }

            var movie = user.UsersMovies.FirstOrDefault(x => x.MovieId == movieId);

            if (movie != null)
            {
                user.UsersMovies.Remove(movie);

                await context.SaveChangesAsync();
            }
        }
    }
}
