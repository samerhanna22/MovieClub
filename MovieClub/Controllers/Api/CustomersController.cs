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
    public class CustomersController : ApiController
    {
        ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        //// GET api/<controller>
        //public IEnumerable<CustomerDto> GetCustomers()
        //{
        //    return _context.Customers
        //        .Include(c => c.MembershipType)
        //        .ToList()
        //        .Select(Mapper.Map<Customer,CustomerDto>);
        //}

        // GET /api/customers
        public IHttpActionResult GetCustomers(string query = null)
        {
            var customersQuery = _context.Customers
                .Include(c => c.MembershipType);

            if (!String.IsNullOrWhiteSpace(query))
                customersQuery = customersQuery.Where(c => c.Name.Contains(query));

            var customerDtos = customersQuery
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }

        // GET api/<controller>/5
        public IHttpActionResult GetCustomer(int id)
        {
            var CustomerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (CustomerInDb == null)
                return NotFound(); // throw new HttpResponseException(HttpStatusCode.NotFound);

            return Ok( Mapper.Map<Customer, CustomerDto>(CustomerInDb));

        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult CreateCusomter(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(); // throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customer = Mapper.Map<CustomerDto,Customer>(customerDto);

            _context.Customers.Add(customer);
            _context.SaveChanges();

            // update the generated id 
            customerDto.Id = customer.Id;

          
            return Created(new Uri(Request.RequestUri+ "/"+ customer.Id), customerDto);

        }

        // PUT api/<controller>/5
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDto customerDto)
        {
            // make sure that the passed object is valid
            if (!ModelState.IsValid)
                return BadRequest();// throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);


            // make sure that we have customer of such id in the database
            var CustomerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (CustomerInDb == null)
                return NotFound(); // throw new HttpResponseException(HttpStatusCode.NotFound);

            // Mapper.Map<CustomerDto, Customer>(customerDto, CustomerInDb); <-- the <CustomerDto, Customer> are grayout because mapper can determine that from the passed objects so we do not need them
            Mapper.Map(customerDto, CustomerInDb);


            // since we did it in Mapper, we do not need to do it manually
            //CustomerInDb.Name = customer.Name;
            //CustomerInDb.DOB = customer.DOB;
            //CustomerInDb.MembershipTypeId = customer.MembershipTypeId;
            //CustomerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;

            _context.SaveChanges();

            return Ok();


        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public void DeleteCustomer(int id)
        {

            // make sure that we have customer of such id in the database
            var CustomerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (CustomerInDb == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(CustomerInDb);
            _context.SaveChanges();


        }
    }
}