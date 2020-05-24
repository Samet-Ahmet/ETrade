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
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterDtoValidator()
        {
            RuleFor(s => s.FirstName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.LastName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.GenderId).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.Email).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.Email).EmailAddress().WithMessage(Messages.InvalidEmail);
            RuleFor(s => s.PhoneNumber).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.Password).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.PasswordConfirm).NotEmpty().WithMessage(Messages.MustBeFilled);
        }
    }
}
