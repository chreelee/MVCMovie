
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovie.Models;
using MvcMovie.Helpers;
using MvcMovie.Features.Movies.Models;
using MvcMovie.Features.Movies.Services;

namespace MvcMovie.Features.Movies.Controllers
{
    // Route maps to the path that will be used for accessing site, /movies/
    [Route("movies")]
    
    public class MoviesController : Controller
    {
        private readonly IMovieService _movies;
        // add logger
        private readonly ILogger<MoviesController> _logger;

        // every action thru CRUD uses the _context = context query to save data
        // add logger parameter
        public MoviesController(IMovieService movies, ILogger<MoviesController> logger)
        {
            _movies = movies;
            _logger = logger;
        }

        // GET /movies
        [HttpGet("")]

        // overloaded method, public asynchronous method to Search, returns a 'promise'
        // is a Task with an IActionResult object inside it
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // can get rid of query and checks
            // store all movies in database into all
            IEnumerable<Movie> all = await _movies.GetAllAsync();
            IEnumerable<Movie> movies = all; // creating for later use for filtering
            IEnumerable<string?> genreQuery = all.Select(movie => movie.Genre).Distinct(); // include null from Movie.cs

            // if searchString isn't null or empty...
            if (!string.IsNullOrEmpty(searchString))
            {
                // use a filtering operation to search title, where it isnt null
                movies = movies.Where(s => s.Title != null
                                        && s.Title.ToUpper().Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                        );
                // add logging
                _logger.Info("Searching by {searchString}", searchString);
            }

            // if movieGenre isn't null or empty...
            if (!string.IsNullOrEmpty(movieGenre))
            {
                // filters on the basis where movie.Genre is equal to movieGenre filter selected
                movies = movies.Where(movie => movie.Genre == movieGenre);
                _logger.Info("Searching by genre {moviesGenre}", movieGenre);
            }

            // get the viewmodel for the page
            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(genreQuery),
                Movies = movies.ToList()
            };

            // await return view
            return View(movieGenreVM);
        }



        // GET: movies/details/5, loads a single entity by id and shows the view
        [HttpGet("/details/{id:int}", Name ="MovieDetails")]

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("Displaying details for movie {id}", id);
            return View(movie); // returns to the view
        }

        // GET: movies/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            _logger.Info("Create GET");
            return View();
        }

        // POST: movies/create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (!ModelState.IsValid) // if not balid
            {
                _logger.Warn("Create POST model invalid");
                return View(movie);

            }
            // otherwise is valid
            await _movies.AddAsync(movie);
            return RedirectToAction(nameof(Index));
        }


        // GET: movies/edit/5
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {

            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("Edit GET, movie {id}", id);
            return View(movie);
        }


        // POST: movies/edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warn("Edit POST model is invalid. movie id: {movieId}, model id{modelId}", id, movie.Id);
                return View(movie);
            }

            await _movies.UpdateAsync(movie);
            return RedirectToAction(nameof(Index));
        }


        // GET: movies/delete/5
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("DELETE GET, movie {id}", id);
            return View(movie);

        }


        // POST: movies/delete/5
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movies.DeleteAsync(id); // delete the movie
            return RedirectToAction(nameof(Index)); // go back to the index
        }

        // adding new tasks to get movies by their genre alphabetically
        // GET: /movies/bygenre/comedy
        [HttpGet("bygenre/{genre}")]
        public async Task<IActionResult> ByGenre(string genre)
        {
            // get all the movies
            var all = await _movies.GetAllAsync();
            // filtered version of all
            var movies = all.Where(Movie => Movie.Genre != null && string.Equals(Movie.Genre, genre, StringComparison.OrdinalIgnoreCase));
            var viewModel = new MovieGenreViewModel
            {
                Genres = new SelectList(all.Select(m => m.Genre).Distinct()),
                Movies = movies.ToList(),
                MovieGenre = genre
            };

            return View("Index", viewModel);
        }

        // get movies released in a certain time
        // GET: /movies/released/2010/5
        [HttpGet("released/{year:int:min(1900)}/{month:int:range(1,12)?}")]
        public async Task<IActionResult> Released(int year, int month)
        {
            // get all the movies
            var all = await _movies.GetAllAsync();
            // filtered version of all
            var movies = all.Where(Movie => Movie.ReleaseDate.Year == year && (month == 0 ? true : Movie.ReleaseDate.Month == month));
            var viewModel = new MovieGenreViewModel
            {
                Genres = new SelectList(all.Select(m => m.Genre).Distinct()),
                Movies = movies.ToList(),
            };

            return View("Index", viewModel);
        }

    }
}
