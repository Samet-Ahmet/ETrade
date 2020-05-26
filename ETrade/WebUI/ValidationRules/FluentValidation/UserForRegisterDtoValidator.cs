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
            RuleFor(s => s.FirstName).Length(0,30).WithMessage(Messages.Max30Characters);
            RuleFor(s => s.FirstName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(s => s.LastName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.LastName).Length(0,30).WithMessage(Messages.Max30Characters);
            RuleFor(s => s.FirstName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(s => s.GenderId).NotEmpty().WithMessage(Messages.MustBeFilled);

            RuleFor(s => s.Email).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.Email).EmailAddress().WithMessage(Messages.InvalidEmail);
            RuleFor(s => s.Email).Length(0,50).WithMessage(Messages.Max50Characters);

            RuleFor(s => s.PhoneNumber).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.PhoneNumber).Length(10).WithMessage(Messages.PhoneError);
            RuleFor(s => s.PhoneNumber).Matches(@"^[1-9]\d{2}\d{3}\d{4}$").WithMessage(Messages.PhoneError);
            
            RuleFor(s => s.Password).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(s => s.PasswordConfirm).NotEmpty().WithMessage(Messages.MustBeFilled);
        }
    }
}
