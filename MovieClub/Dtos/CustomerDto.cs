using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MovieClub.Models;

namespace MovieClub.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please enter customer's name")]
        [MaxLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsLetter { get; set; }


        public byte MembershipTypeId { get; set; } // sometimes we load the membership type id only and not the whole class of MembershipType

        public MembershipTypeDto MembershipType { get; set; }

        //[Min18YearsIsAMember] <-- we commented this temporarly because it will cause an issue with the dto since in that attr we are casting to Customer and not CustomerDto
        public DateTime? DOB { get; set; }
    }
}