using FluentValidation;
using MT.OnlineRestaurant.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.OnlineRestaurant.BusinessLayer.ModelValidator
{
    public class CustomerValidator : AbstractValidator<CustomerDetails>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Firstname is required");


            RuleFor(m => m.LastName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lastname is required");


            RuleFor(m => m.Password)
              .NotEmpty()
              .NotNull()
              .WithMessage("Password  is required");

            RuleFor(m => m.MobileNumber)
              .NotEmpty()
              .NotNull()
              .WithMessage("Mobile  is required");

            RuleFor(m => m.IsActive)
              .NotEmpty()
              .NotNull();

            RuleFor(m => m.UserName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100).WithMessage("UserName must be less than 100 characters");

        }
    }
}
