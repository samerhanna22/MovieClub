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
    public class RentalsController : ApiController
    {

        ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            // id is the customer id
            var CustomerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (CustomerInDb == null)
                return NotFound(); // throw new HttpResponseException(HttpStatusCode.NotFound);


            // WeakReference will prepare CustomerRentalsDto to return

            var Rentals = _context.Rentals.Include( c => c.Movie).Where(r => r.Customer.Id == id).ToList();

            CustomerRentalsDto custRents = new CustomerRentalsDto()
            {
                Customer = Mapper.Map<Customer, CustomerDto>(CustomerInDb),
                Rentals = Rentals
            };

            return Ok(custRents);

        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult CreateNewRentals(RentalDto newRental)
        {
            // we loop through the list of movies ids and we create rental domain model item for each
            // then we store in the database
            // load the customer first

            // NOTE: we used Single and NOT SingleOrDefault because this is Internal so the customer id is expected to be valid
            //       and in order to prevent from malitious attacks it is better that we have internal server error then explained error
            var customer = _context.Customers.Single(c => c.Id == newRental.CustomerId);

            // this is excellent so we get the movies of provided ids in one sql query
            var movies = _context.Movies.Where(m => newRental.MovieIds.Contains(m.Id)).ToList();
            
            var dateRented = DateTime.Now;

            foreach (Movie movie in movies)
            {
                if (movie != null)
                {
                    if (movie.NumberAvailable == 0)
                        return BadRequest("movie is not available");


                    // since this is an object returned by the _context then when we say SaveChanges() it will update this as well!
                    movie.NumberAvailable--;


                    var rental = new Rental() { Customer = customer, Movie = movie, DateRented = dateRented };
                    _context.Rentals.Add(rental);


                }
            }

            _context.SaveChanges();

            // we are returning Ok() and not Created() because we have many rentals created and not one item that we need to response with
            // so we simply just return Ok
            return Ok();

        }

        // PUT api/<controller>/5
        public void Put(Something ss    )
        {

            //  Console.WriteLine(ss.rentalIds.Count);

            // need to update rentals with returned date = now
            foreach(Rental rent in _context.Rentals.Include( r => r.Customer).Include( r => r.Movie).Where(r => ss.rentalIds.Contains(r.Id)).ToList())
            {
                rent.DateReturned = DateTime.Now;
            }

            _context.SaveChanges();

        }



        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

   public class Something
    {
        public string name { get; set; }
        public List<int> rentalIds { get; set; }
    }
}