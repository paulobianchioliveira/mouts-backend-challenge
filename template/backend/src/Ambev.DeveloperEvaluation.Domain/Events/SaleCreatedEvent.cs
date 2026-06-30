namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a new sale is created
/// </summary>
public class SaleCreatedEvent
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
    /// Gets the customer ID
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the branch ID
    /// </summary>
    public Guid BranchId { get; }

    /// <summary>
    /// Gets the total amount
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// Gets the number of items
    /// </summary>
    public int ItemsCount { get; }

    /// <summary>
    /// Gets the event timestamp
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Creates a new SaleCreatedEvent
    /// </summary>
    public SaleCreatedEvent(Guid saleId, string saleNumber, Guid customerId, Guid branchId, decimal totalAmount, int itemsCount)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        CustomerId = customerId;
        BranchId = branchId;
        TotalAmount = totalAmount;
        ItemsCount = itemsCount;
        OccurredAt = DateTime.UtcNow;
    }
}
