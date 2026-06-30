using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Product entity
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    /// <summary>
    /// Initializes validation rules for Product
    /// </summary>
    public ProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Product name is required and must not exceed 200 characters");

        RuleFor(product => product.Description)
            .MaximumLength(500)
            .WithMessage("Product description must not exceed 500 characters");

        RuleFor(product => product.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero");

        RuleFor(product => product.Category)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Category is required and must not exceed 100 characters");
    }
}
