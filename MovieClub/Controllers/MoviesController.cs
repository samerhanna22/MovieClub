using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MovieClub.Models;
using MovieClub.ViewModels;
using System.Data.Entity;


namespace MovieClub.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing); // 've added this although it is not there in the tutorial

        }

         public ViewResult Index()
        {

            // we will modify this one so that it returns just the view since we are not passing a model to the view because the ajax in the apge will be
            // clling API-MovieController to retrieve the data (list of movies) and render them in JQuery DataTable

            //var movies = _context.Movies.Include(m => m.Genre).ToList();
            // return View(movies);


            if(User.IsInRole(RoleName.CanManageMovies))
                return View("List"); ;
            // else
            return View("ReadOnlyList");

        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(c => c.Genre).ToList().SingleOrDefault(c => c.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
                return HttpNotFound();

            var Genres = _context.Genres.ToList();


            return View("MovieForm", new MovieFormViewModel() { Movie = movie, Genres = Genres });
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var movie = new Movie();
            var Genres = _context.Genres.ToList();

            return View("MovieForm", new MovieFormViewModel() {Movie = movie, Genres = Genres });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Save(Movie movie)
        {

            if (!ModelState.IsValid)
            {
                return View("MovieForm", new MovieFormViewModel() { Movie = movie, Genres = _context.Genres.ToList() });

            }

            if (movie.Id == 0)
            {
                movie.Genre = _context.Genres.SingleOrDefault(g => g.Id == movie.GenreId);
                movie.DateAdded = DateTime.Now;

                _context.Movies.Add(movie);

            }
            else
            {
                var MovieInDb = _context.Movies.Single(c => c.Id == movie.Id);

                MovieInDb.Name = movie.Name;
                MovieInDb.ReleaseDate = movie.ReleaseDate;
                MovieInDb.Genre = _context.Genres.SingleOrDefault(g => g.Id == movie.GenreId);
                
                MovieInDb.NumberInStock = movie.NumberInStock;

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");


        }

        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);
        }
        //// GET: Movies
        //public ActionResult Random()
        //{
        //    var movie = new Movie() { Name = "Shrek" };

        //    var Custoemrs = new List<Customer>() { new Customer() { Name = "Samer", id = 1 }, new Customer() { Name = "Karam", id = 2 }, new Customer() { Name = "Rayan", id = 3 } };

        //    var ViewModel = new MovieClub.ViewModels.RandomMovieViewModel() { Movie = movie, Customers = Custoemrs };

        //    // in earlier MVC they used ViewData and ViewBag to pass data to View
        //    return View(ViewModel); // the View(<model>) is actually doing this: it is creating new ViewResult() and set ViewResult.ViewData.Model to the <model> you've passed to it


        //    // return Content("Hello Smer!");
        //    // return HttpNotFound();
        //    // return new EmptyResult();
        //    //return RedirectToAction("Index", "Home", new { page = "1", sortBy = "name" });
        //}

        //public ActionResult Edit(int ID)
        //{

        //    return Content("ID = " + ID);

        //}

        //public ActionResult Index(int? pageIndex, string sortBy)
        //{

        //    if (!pageIndex.HasValue) pageIndex = 1;
        //    if (String.IsNullOrWhiteSpace(sortBy)) sortBy = "Name";

        //    return Content(String.Format("pageIndex index {0} and the sortby is {1}", pageIndex, sortBy));


        //}

        //// search on google for ASP.NET Attribute MVC Route Constraints for more like range, min, max, ...etc
        //[Route("movies/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
        //public ActionResult ByReleaseDate(int year, int month)
        //{
        //    return Content(String.Format("{0}/{1}", year.ToString(), month.ToString()));
        //}

    }
}