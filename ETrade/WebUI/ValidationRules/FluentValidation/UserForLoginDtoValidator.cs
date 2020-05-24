using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using Entities.Dtos;
using FluentValidation;

namespace WebUI.ValidationRules.FluentValidation
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            RuleFor(ufl => ufl.Email).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(ufl => ufl.Email).EmailAddress().WithMessage(Messages.InvalidEmail);
            RuleFor(ufl => ufl.Password).NotEmpty().WithMessage(Messages.MustBeFilled);
        }
    }
}
