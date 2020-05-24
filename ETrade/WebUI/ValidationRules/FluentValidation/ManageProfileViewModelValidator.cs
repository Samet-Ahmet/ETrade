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
            RuleFor(mp => mp.User.LastName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(mp => mp.User.PhoneNumber).NotEmpty().WithMessage(Messages.MustBeFilled);

        }
    }
}
