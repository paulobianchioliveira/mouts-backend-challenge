using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale aggregate root.
/// Manages sale items and enforces business rules.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the sale number (unique identifier for business purposes)
    /// </summary>
    public string SaleNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets or sets the customer ID (External Identity)
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the customer name (denormalized)
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch ID where the sale was made (External Identity)
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the branch name (denormalized)
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the total sale amount (sum of all items)
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets the total discount amount
    /// </summary>
    public decimal TotalDiscount { get; private set; }

    /// <summary>
    /// Gets the sale status
    /// </summary>
    public SaleStatus Status { get; private set; }

    /// <summary>
    /// Gets or sets the cancellation reason
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Gets or sets the cancellation date
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Gets the sale items collection
    /// </summary>
    private readonly List<SaleItem> _items = new();

    /// <summary>
    /// Gets the readonly collection of sale items
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Navigation properties
    /// </summary>
    public Customer? Customer { get; set; }
    public Branch? Branch { get; set; }

    /// <summary>
    /// Private constructor for EF Core
    /// </summary>
    private Sale() { }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <param name="customerName">Customer name</param>
    /// <param name="branchId">Branch ID</param>
    /// <param name="branchName">Branch name</param>
    /// <returns>New Sale instance</returns>
    public static Sale Create(Guid customerId, string customerName, Guid branchId, string branchName)
    {
        return new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = GenerateSaleNumber(),
            SaleDate = DateTime.UtcNow,
            CustomerId = customerId,
            CustomerName = customerName,
            BranchId = branchId,
            BranchName = branchName,
            Status = SaleStatus.Active,
            TotalAmount = 0,
            TotalDiscount = 0
        };
    }

    /// <summary>
    /// Adds an item to the sale
    /// </summary>
    /// <param name="item">Sale item to add</param>
    public void AddItem(SaleItem item)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot add items to a cancelled sale.");
        }

        item.SaleId = Id;
        _items.Add(item);
        RecalculateTotals();
    }

    /// <summary>
    /// Removes an item from the sale
    /// </summary>
    /// <param name="itemId">Item ID to remove</param>
    public void RemoveItem(Guid itemId)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot remove items from a cancelled sale.");
        }

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _items.Remove(item);
            RecalculateTotals();
        }
    }

    /// <summary>
    /// Updates an existing item quantity
    /// </summary>
    /// <param name="itemId">Item ID to update</param>
    /// <param name="newQuantity">New quantity</param>
    public void UpdateItemQuantity(Guid itemId, int newQuantity)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot update items in a cancelled sale.");
        }

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new ArgumentException($"Item with ID {itemId} not found.", nameof(itemId));
        }

        item.UpdateQuantity(newQuantity);
        RecalculateTotals();
    }

    /// <summary>
    /// Cancels a specific item in the sale
    /// </summary>
    /// <param name="itemId">Item ID to cancel</param>
    /// <param name="reason">Cancellation reason</param>
    public void CancelItem(Guid itemId, string reason)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot cancel items in an already cancelled sale.");
        }

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new ArgumentException($"Item with ID {itemId} not found.", nameof(itemId));
        }

        item.Cancel(reason);
        RecalculateTotals();
    }

    /// <summary>
    /// Cancels the entire sale
    /// </summary>
    /// <param name="reason">Cancellation reason</param>
    public void Cancel(string reason)
    {
        if (Status == SaleStatus.Cancelled)
        {
            throw new InvalidOperationException("Sale is already cancelled.");
        }

        Status = SaleStatus.Cancelled;
        CancellationReason = reason;
        CancelledAt = DateTime.UtcNow;

        // Cancel all items
        foreach (var item in _items.Where(i => !i.IsCancelled))
        {
            item.Cancel(reason);
        }

        RecalculateTotals();
    }

    /// <summary>
    /// Recalculates total amounts considering only active items
    /// </summary>
    private void RecalculateTotals()
    {
        var activeItems = _items.Where(i => !i.IsCancelled).ToList();
        TotalAmount = activeItems.Sum(i => i.TotalAmount);
        TotalDiscount = activeItems.Sum(i => i.DiscountAmount);
    }

    /// <summary>
    /// Generates a unique sale number
    /// Format: SALE-YYYYMMDD-HHMMSS-GUID
    /// </summary>
    private static string GenerateSaleNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
        var uniqueId = Guid.NewGuid().ToString("N")[..8].ToUpper();
        return $"SALE-{timestamp}-{uniqueId}";
    }

    /// <summary>
    /// Gets whether the sale is cancelled
    /// </summary>
    public bool IsCancelled => Status == SaleStatus.Cancelled;

    /// <summary>
    /// Gets the count of active items (non-cancelled)
    /// </summary>
    public int ActiveItemsCount => _items.Count(i => !i.IsCancelled);

    /// <summary>
    /// Gets the count of total items (including cancelled)
    /// </summary>
    public int TotalItemsCount => _items.Count;
}
