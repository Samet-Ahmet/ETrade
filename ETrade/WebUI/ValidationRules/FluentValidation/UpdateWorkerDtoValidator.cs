using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using Entities.Dtos;
using FluentValidation;

namespace WebUI.ValidationRules.FluentValidation
{
    public class UpdateWorkerDtoValidator : AbstractValidator<UpdateWorkerDto>
    {
        public UpdateWorkerDtoValidator()
        {
            RuleFor(uw => uw.FirstName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(uw => uw.FirstName).Length(0, 30).WithMessage(Messages.Max30Characters);
            RuleFor(uw => uw.FirstName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(uw => uw.LastName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(uw => uw.LastName).Length(0, 30).WithMessage(Messages.Max30Characters);
            RuleFor(uw => uw.LastName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(uw => uw.PhoneNumber).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(uw => uw.PhoneNumber).Length(10).WithMessage(Messages.PhoneError);
            RuleFor(uw => uw.PhoneNumber).Matches(@"^[1-9]\d{2}\d{3}\d{4}$").WithMessage(Messages.PhoneError);

            RuleFor(uw => uw.Street).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(uw => uw.Street).Length(0, 30).WithMessage(Messages.Max30Characters);

            RuleFor(uw => uw.AddressNumber).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(uw => uw.AddressNumber).Length(0, 30).WithMessage(Messages.Max30Characters);

            RuleFor(uw => uw.CityId).NotEmpty().WithMessage(Messages.MustBeFilled);

            RuleFor(uw => uw.DistrictId).NotEmpty().WithMessage(Messages.MustBeFilled);
        }
    }
}
