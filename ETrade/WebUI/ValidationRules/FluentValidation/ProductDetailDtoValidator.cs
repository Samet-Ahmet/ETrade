using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Constants;
using Entities.Dtos;
using FluentValidation;

namespace WebUI.ValidationRules.FluentValidation
{
    public class ProductDetailDtoValidator : AbstractValidator<ProductDetailDto>
    {
        public ProductDetailDtoValidator()
        {
            RuleFor(pd => pd.Product.ProductName).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(pd => pd.Product.BrandId).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(pd => pd.Product.ProductDef).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(pd => pd.Product.UnitPrice).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(pd => pd.Product.UnitsInStock).NotEmpty().WithMessage(Messages.MustBeFilled);
            RuleFor(pd => pd.Product.CategoryId).NotEmpty().WithMessage(Messages.MustBeFilled);
        }
    }
}
