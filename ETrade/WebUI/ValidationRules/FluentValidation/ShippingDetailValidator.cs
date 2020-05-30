using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using Entities.Concrete;
using FluentValidation;

namespace WebUI.ValidationRules.FluentValidation
{
    public class ShippingDetailValidator : AbstractValidator<ShippingDetail>
    {
        public ShippingDetailValidator()
        {
            RuleFor(sd => sd.AddressName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(sd => sd.AddressName).Length(0,50).WithMessage(Messages.Max50Characters);


            RuleFor(sd => sd.Number).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(sd => sd.Number).Length(0, 15).WithMessage(Messages.Max15Characters);

            RuleFor(sd => sd.Street).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(sd => sd.Street).Length(0, 50).WithMessage(Messages.Max50Characters);
        }
    }
}
