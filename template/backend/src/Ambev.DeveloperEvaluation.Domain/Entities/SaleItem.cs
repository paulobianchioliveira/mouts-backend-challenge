using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale with discount calculation logic.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale ID this item belongs to
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name (denormalized for performance)
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of items
    /// Business Rule: Maximum 20 items per product
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets or sets the unit price at the time of sale
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount percentage applied (0-20%)
    /// Business Rules:
    /// - 4-9 items: 10% discount
    /// - 10-20 items: 20% discount
    /// - Below 4 or above 20: No discount
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Gets the discount amount in monetary value
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Gets the total amount for this item (quantity * unitPrice - discount)
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the cancellation reason
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Gets or sets the cancellation date
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Navigation property to Sale
    /// </summary>
    public Sale? Sale { get; set; }

    /// <summary>
    /// Navigation property to Product
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Private constructor for EF Core
    /// </summary>
    private SaleItem() { }

    /// <summary>
    /// Creates a new sale item with automatic discount calculation
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <param name="productName">Product name</param>
    /// <param name="quantity">Quantity (1-20)</param>
    /// <param name="unitPrice">Unit price</param>
    /// <returns>New SaleItem instance</returns>
    /// <exception cref="ArgumentException">When quantity is invalid</exception>
    public static SaleItem Create(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (!Discount.IsValidQuantity(quantity))
        {
            throw new ArgumentException($"Invalid quantity: {quantity}. Must be between 1 and 20.", nameof(quantity));
        }

        if (unitPrice <= 0)
        {
            throw new ArgumentException("Unit price must be greater than zero.", nameof(unitPrice));
        }

        var discount = Discount.Calculate(quantity, unitPrice);

        var item = new SaleItem
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice,
            DiscountPercentage = discount.Percentage,
            DiscountAmount = discount.Amount,
            IsCancelled = false
        };

        item.CalculateTotalAmount();

        return item;
    }

    /// <summary>
    /// Updates the quantity and recalculates discount and total
    /// </summary>
    /// <param name="newQuantity">New quantity (1-20)</param>
    /// <exception cref="ArgumentException">When quantity is invalid</exception>
    public void UpdateQuantity(int newQuantity)
    {
        if (!Discount.IsValidQuantity(newQuantity))
        {
            throw new ArgumentException($"Invalid quantity: {newQuantity}. Must be between 1 and 20.", nameof(newQuantity));
        }

        Quantity = newQuantity;
        var discount = Discount.Calculate(newQuantity, UnitPrice);
        DiscountPercentage = discount.Percentage;
        DiscountAmount = discount.Amount;
        CalculateTotalAmount();
    }

    /// <summary>
    /// Cancels this item
    /// </summary>
    /// <param name="reason">Cancellation reason</param>
    public void Cancel(string reason)
    {
        IsCancelled = true;
        CancellationReason = reason;
        CancelledAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount for this item
    /// </summary>
    private void CalculateTotalAmount()
    {
        var subtotal = Quantity * UnitPrice;
        TotalAmount = subtotal - DiscountAmount;
    }

    /// <summary>
    /// Gets the subtotal before discount
    /// </summary>
    public decimal GetSubtotal() => Quantity * UnitPrice;
}
