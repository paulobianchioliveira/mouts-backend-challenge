using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for SaleItem entity
/// </summary>
public class SaleItemValidator : AbstractValidator<SaleItem>
{
    /// <summary>
    /// Initializes validation rules for SaleItem
    /// </summary>
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(item => item.ProductName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Product name is required and must not exceed 200 characters");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Quantity cannot exceed 20 items per product");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero");

        RuleFor(item => item.DiscountPercentage)
            .InclusiveBetween(0, 100)
            .WithMessage("Discount percentage must be between 0 and 100");

        RuleFor(item => item)
            .Must(HaveValidDiscount)
            .WithMessage("Discount percentage does not match business rules for the given quantity");
    }

    /// <summary>
    /// Validates if the discount matches business rules
    /// </summary>
    private bool HaveValidDiscount(SaleItem item)
    {
        var expectedDiscount = Discount.GetDiscountPercentage(item.Quantity);
        return item.DiscountPercentage == expectedDiscount;
    }
}
