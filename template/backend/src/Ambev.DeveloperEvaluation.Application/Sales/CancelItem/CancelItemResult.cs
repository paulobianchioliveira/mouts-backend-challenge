namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Result returned after cancelling an item
/// </summary>
public class CancelItemResult
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the item ID
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets whether the item was successfully cancelled
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the new total amount of the sale
    /// </summary>
    public decimal NewSaleTotalAmount { get; set; }
}
