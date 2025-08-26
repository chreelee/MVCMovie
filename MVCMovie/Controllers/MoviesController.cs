using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using MvcMovie.Models;
using MvcMovie.Helpers;

namespace MVCMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;
        // add logger
        private readonly ILogger<MoviesController> _logger;

        // every action thru CRUD uses the _context = context query to save data
        // add logger parameter
        public MoviesController(MvcMovieContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // overloaded method, public asynchronous method to Search, returns a 'promise'
        // is a Task with an IActionResult object inside it
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            if (_context.Movie == null) // if movie inside of context is null
            {
                _logger.Error(new NullReferenceException(), "Move in context is null");
                return Problem("Entity set 'MvcMovieContext.Movie'  is null."); // raise problem
            }

            // add genre query
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            // assuming it's not null... continue
            // use LINQ to read movies
            var movies = from m in _context.Movie select m;

            // if searchString isn't null or empty...
            if (!String.IsNullOrEmpty(searchString))
            {
                // use a filtering operation with lambda => to send it to upper case
                movies = movies.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            // if movieGenre isn't null or empty...
            if (!String.IsNullOrEmpty(movieGenre))
            {
                // 
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };


            // await return view
            return View(movieGenreVM);
        }



        // GET: Movies/Details/5
        // loads a single entity by id and shows the view
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.Warn("Details called with null id");
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                _logger.Warn("Movie {id} not found", id);
                return NotFound();
            }

            _logger.Info("Details displayed for movie {id}", id);
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
            if (ModelState.IsValid)
            {
                _logger.Warn("Create POST model invalid");
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
                // changed text to string interpolation to avoid using argument
                _logger.Warn("Edit GET with null id");
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                _logger.Warn("Edit GET, id {id} not found", id);
                return NotFound();
            }
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
            if (id != movie.Id)
            {
                _logger.Warn("Edit POST id mismatch. movie id: {movieId}, model id{modelId}", id, movie.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    _logger.Info("Edited movie {Id}");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!MovieExists(movie.Id))
                    {
                        _logger.Warn("Concurrency edit failed. Movie {Id} missing", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.Error(ex, "Concurrency exception editing movie {Id}", id);
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

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
