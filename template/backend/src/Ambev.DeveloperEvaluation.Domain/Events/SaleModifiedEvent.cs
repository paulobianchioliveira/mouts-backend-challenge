namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a sale is modified
/// </summary>
public class SaleModifiedEvent
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
    /// Gets the previous total amount
    /// </summary>
    public decimal PreviousTotalAmount { get; }

    /// <summary>
    /// Gets the new total amount
    /// </summary>
    public decimal NewTotalAmount { get; }

    /// <summary>
    /// Gets the modification reason/description
    /// </summary>
    public string ModificationReason { get; }

    /// <summary>
    /// Gets the event timestamp
    /// </summary>
    public DateTime OccurredAt { get; }

    /// <summary>
    /// Creates a new SaleModifiedEvent
    /// </summary>
    public SaleModifiedEvent(Guid saleId, string saleNumber, decimal previousTotalAmount, decimal newTotalAmount, string modificationReason)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        PreviousTotalAmount = previousTotalAmount;
        NewTotalAmount = newTotalAmount;
        ModificationReason = modificationReason;
        OccurredAt = DateTime.UtcNow;
    }
}
