namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is cancelled
/// </summary>
public class SaleCancelledEvent
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
    /// Gets the cancellation reason
    /// </summary>
    public string CancellationReason { get; }

    /// <summary>
    /// Gets the total amount of the cancelled sale
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// Gets the event timestamp
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Creates a new SaleCancelledEvent
    /// </summary>
    public SaleCancelledEvent(Guid saleId, string saleNumber, string cancellationReason, decimal totalAmount)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        CancellationReason = cancellationReason;
        TotalAmount = totalAmount;
        OccurredAt = DateTime.UtcNow;
    }
}
