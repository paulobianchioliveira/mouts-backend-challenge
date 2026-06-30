using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Validator for CancelItemCommand
/// </summary>
public class CancelItemValidator : AbstractValidator<CancelItemCommand>
{
    /// <summary>
    /// Initializes validation rules for CancelItemCommand
    /// </summary>
    public CancelItemValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Cancellation reason is required and must not exceed 500 characters");
    }
}
