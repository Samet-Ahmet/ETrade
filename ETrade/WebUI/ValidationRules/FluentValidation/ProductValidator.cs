using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using Entities.Concrete;
using FluentValidation;

namespace WebUI.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.BrandId).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.ProductDef).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.UnitPrice).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.UnitsInStock).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage(Messages.MustBeFilled);
        }
    }
}
