using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using Entities.Dtos;
using FluentValidation;
using WebUI.Models;

namespace WebUI.ValidationRules.FluentValidation
{
    public class AddWorkerDtoValidator : AbstractValidator<AddWorkerDto>
    {
        public AddWorkerDtoValidator()
        {
            RuleFor(aw => aw.IdentityNo).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(aw => aw.IdentityNo).GreaterThanOrEqualTo(10000000000).WithMessage(Messages.TcError);
            RuleFor(aw => aw.IdentityNo).LessThanOrEqualTo(100000000000).WithMessage(Messages.TcError);

            RuleFor(aw => aw.FirstName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(aw => aw.FirstName).Length(0, 30).WithMessage(Messages.Max30Characters);
            RuleFor(aw => aw.FirstName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(aw => aw.LastName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(aw => aw.LastName).Length(0, 30).WithMessage(Messages.Max30Characters);
            RuleFor(aw => aw.LastName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(aw => aw.Email).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(aw => aw.Email).Length(0, 50).WithMessage(Messages.Max50Characters);
            RuleFor(aw => aw.Email).EmailAddress().WithMessage(Messages.InvalidEmail);

            RuleFor(aw => aw.PhoneNumber).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(aw => aw.PhoneNumber).Length(10).WithMessage(Messages.PhoneError);
            RuleFor(aw => aw.PhoneNumber).Matches(@"^[1-9]\d{2}\d{3}\d{4}$").WithMessage(Messages.PhoneError);

            RuleFor(aw => aw.Street).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(aw => aw.Street).Length(0, 30).WithMessage(Messages.Max30Characters);

            RuleFor(aw => aw.AddressNumber).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(aw => aw.AddressNumber).Length(0, 30).WithMessage(Messages.Max30Characters);

            RuleFor(aw => aw.CityId).NotEmpty().WithMessage(Messages.MustBeFilled);

            RuleFor(aw => aw.DistrictId).NotEmpty().WithMessage(Messages.MustBeFilled);
        }
    }
}
