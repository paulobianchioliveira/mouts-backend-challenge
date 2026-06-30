using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Validator for ListSalesQuery
/// </summary>
public class ListSalesValidator : AbstractValidator<ListSalesQuery>
{
    /// <summary>
    /// Initializes validation rules for ListSalesQuery
    /// </summary>
    public ListSalesValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x)
            .Must(x => x.StartDate == null || x.EndDate == null || x.StartDate <= x.EndDate)
            .WithMessage("Start date must be less than or equal to end date");
    }
}
