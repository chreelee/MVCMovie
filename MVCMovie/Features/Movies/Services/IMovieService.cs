// interface contains all movie operations
using System.Collections.Generic;
using System.Threading.Tasks; // using async methods
using MvcMovie.Features.Movies.Models;

namespace MvcMovie.Features.Movies.Services
{
    public interface IMovieService // changed from class
    {
        // operation that will get enumeration of all movies in database, is a TASK
        Task<IEnumerable<Movie>> GetAllAsync();

        // get a movie, which may be null
        Task<Movie?> GetByIdAsync(int id);

        // add a movie
        Task AddAsync(Movie movie);

        // overwrite existing entry in database
        Task UpdateAsync(Movie movie);

        // delete entry
        Task DeleteAsync(int id);
    }
}
