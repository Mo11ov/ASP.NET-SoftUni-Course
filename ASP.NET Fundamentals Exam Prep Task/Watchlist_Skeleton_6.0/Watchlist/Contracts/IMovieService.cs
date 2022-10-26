using Watchlist.Data.Entities;
using Watchlist.Models;

namespace Watchlist.Contracts
{
    public interface IMovieService
    {
        Task AddMovieAsync(AddMovieViewModel model);

        Task<ICollection<Genre>> GetGenresAsync();

        Task<ICollection<MoviesViewModel>> GetMoviesAsync();

        Task AddToUserWatchedMoviesAsync(int movieId, string userID);

        Task<ICollection<MoviesViewModel>> GetUserWatchedMovies(string userId);

        Task RemoveFromUserCollection(int movieId, string userId);
    }
}
