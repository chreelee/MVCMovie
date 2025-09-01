
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovie.Models;
using MvcMovie.Helpers;
using MvcMovie.Services;

namespace MVCMovie.Controllers
{
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
            if (!String.IsNullOrEmpty(searchString))
            {
                // use a filtering operation to search title, where it isnt null
                movies = movies.Where(s => s.Title != null
                                        && s.Title.ToUpper().Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                        );
                // add logging
                _logger.Info("Searching by {searchString}", searchString);
            }

            // if movieGenre isn't null or empty...
            if (!String.IsNullOrEmpty(movieGenre))
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



        // GET: Movies/Details/5, loads a single entity by id and shows the view
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("Displaying details for movie {id}", id);
            return View(movie); // returns to the view
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            _logger.Info("Create GET");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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


        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("Edit GET, movie {id}", id);
            return View(movie);
        }


        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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


        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("DELETE GET, movie {id}", id);
            return View(movie);

        }


        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _movies.DeleteAsync(id); // delete the movie
            return RedirectToAction(nameof(Index)); // go back to the index
        }

    }
}
