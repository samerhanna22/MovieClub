using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieClub.Models
{
    public class Min18YearsIsAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            if (customer.MembershipTypeId == 1) return ValidationResult.Success;

            if (customer.DOB == null)
                return new ValidationResult("Date of Birth is required for membership");

            if ((DateTime.Now.Year - customer.DOB.Value.Year) >= 18) return ValidationResult.Success;

            return new ValidationResult("Customer should be 18+ years old to be a member ");
        }
    }
}