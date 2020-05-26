using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using WebUI.Models;

namespace WebUI.ValidationRules.FluentValidation
{
    public class ManageProfileViewModelValidator : AbstractValidator<ManageProfileViewModel>
    {
        public ManageProfileViewModelValidator()
        {
            RuleFor(mp => mp.User.FirstName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(mp => mp.User.FirstName).Length(0, 30).WithMessage(Messages.Max30Characters);
            RuleFor(mp => mp.User.FirstName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(mp => mp.User.LastName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(mp => mp.User.LastName).Length(0, 30).WithMessage(Messages.Max30Characters);
            RuleFor(mp => mp.User.LastName).Matches(@"^[a-zA-Z öÖçÇüÜğĞşŞİı]+$").WithMessage(Messages.OnlyLetters);

            RuleFor(mp => mp.User.PhoneNumber).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(mp => mp.User.PhoneNumber).Length(10).WithMessage(Messages.PhoneError);
            RuleFor(mp => mp.User.PhoneNumber).Matches(@"^[2-9]\d{2}\d{3}\d{4}$").WithMessage(Messages.PhoneError);

        }
    }
}
