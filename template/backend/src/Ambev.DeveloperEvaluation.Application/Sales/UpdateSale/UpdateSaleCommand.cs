using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command for updating an existing sale
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Gets or sets the sale ID to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the list of items (replaces existing items)
    /// </summary>
    public List<CreateSaleItemDto> Items { get; set; } = new();
}
