namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when an item in a sale is cancelled
/// </summary>
public class ItemCancelledEvent
{
    /// <summary>
    /// Gets the sale ID
    /// </summary>
    public Guid SaleId { get; }

    /// <summary>
    /// Gets the sale number
    /// </summary>
    public string SaleNumber { get; }

    /// <summary>
    /// Gets the item ID
    /// </summary>
    public Guid ItemId { get; }

    /// <summary>
    /// Gets the product name
    /// </summary>
    public string ProductName { get; }

    /// <summary>
    /// Gets the quantity cancelled
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the cancellation reason
    /// </summary>
    public string CancellationReason { get; }

    /// <summary>
    /// Gets the item total amount
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// Gets the event timestamp
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Creates a new ItemCancelledEvent
    /// </summary>
    public ItemCancelledEvent(Guid saleId, string saleNumber, Guid itemId, string productName, int quantity, string cancellationReason, decimal totalAmount)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        ItemId = itemId;
        ProductName = productName;
        Quantity = quantity;
        CancellationReason = cancellationReason;
        TotalAmount = totalAmount;
        OccurredAt = DateTime.UtcNow;
    }
}
