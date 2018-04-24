using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieClub.Models;

namespace MovieClub.Dtos
{
    public class CustomerRentalsDto
    {

        public CustomerDto Customer { get; set; }
        public List<Rental> Rentals { get; set; }

    }
}