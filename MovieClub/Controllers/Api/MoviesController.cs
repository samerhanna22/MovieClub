using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieClub.Dtos;
using MovieClub.Models;
using System.Data.Entity;

namespace MovieClub.Controllers.Api
{
    public class MoviesController : ApiController
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

        // GET api/<controller>
        public IEnumerable<MovieDto> GetMovies(string query = null)
        {
            //return _context.Movies.Include(c => c.Genre).ToList().Select(Mapper.Map<Movie, MovieDto>);

            var movieQuery = _context.Movies.Include(c => c.Genre).Where( m => m.NumberAvailable > 0);

            if (!String.IsNullOrWhiteSpace(query))
                movieQuery = movieQuery.Where(c => c.Name.Contains(query));

            var moviesDtos = movieQuery.ToList().Select(Mapper.Map<Movie, MovieDto>);

            return moviesDtos;

        }

        // GET api/<controller>/5
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.ToList().SingleOrDefault(c => c.Id == id);

            if (movie == null)
                return NotFound(); //

            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(); // throw new HttpResponseException(HttpStatusCode.BadRequest);


            var movie = Mapper.Map<MovieDto, Movie>(movieDto);

            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;

            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(); //  throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movie = Mapper.Map<MovieDto, Movie>(movieDto);


            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movieInDb == null)
                return NotFound(); //   throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(movieDto, movieInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public void Delete(int id)
        {
            var movieInDb = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movieInDb == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();
        }
    }
}