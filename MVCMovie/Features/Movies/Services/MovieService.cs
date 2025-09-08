// the class that interacts with the databases, using the interface
using Microsoft.EntityFrameworkCore;
using MvcMovie.Features.Movies.Models;
using MVCMovie.Data;
using MVCMovie.Models;

namespace MvcMovie.Features.Movies.Services
{
    // extends IMovieService
    public class MovieService : IMovieService
    {
        private readonly MvcMovieContext _db;

        public MovieService(MvcMovieContext db) => _db = db;

        // have to send back a promise
        public async Task<IEnumerable<Movie>> GetAllAsync() => await _db.Movie.ToListAsync();

        // will look at database, get it' ID, turn it into a movie object and return it
        public async Task<Movie?> GetByIdAsync(int id) => await _db.Movie.FirstOrDefaultAsync(movie => movie.Id == id);


        public async Task AddAsync(Movie movie)
        {
            _db.Movie.Add(movie); // add movie
            await _db.SaveChangesAsync(); // make sure change is synced
        }
        public async Task UpdateAsync(Movie movie)
        {
            _db.Movie.Update(movie);
            await _db.SaveChangesAsync(); // make sure change is synced       
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Movie.FindAsync(id);
            if (entity is null) return;
            // if entity is NOT null
            _db.Movie.Remove(entity); // remove it
            await _db.SaveChangesAsync(); // make sure change is synced  
        }

    }
}
