using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using WebUI.Models;

namespace WebUI.ValidationRules.FluentValidation
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(p => p.OldPassword).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.NewPassword).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.NewPasswordConfirm).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.NewPasswordConfirm)
                .Equal(p => p.NewPassword).WithMessage(Messages.PasswordConfirmError);
            RuleFor(p => p.NewPassword)
                .NotEqual(p => p.OldPassword).WithMessage(Messages.SamePassword);
        }
    }
}
