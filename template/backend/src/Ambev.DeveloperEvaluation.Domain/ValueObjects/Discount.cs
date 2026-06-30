namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

/// <summary>
/// Value object representing a discount with business rules.
/// </summary>
public class Discount
{
    /// <summary>
    /// Gets the discount percentage (0-100)
    /// </summary>
    public decimal Percentage { get; private set; }

    /// <summary>
    /// Gets the discount amount in monetary value
    /// </summary>
    public decimal Amount { get; private set; }

    private Discount(decimal percentage, decimal amount)
    {
        Percentage = percentage;
        Amount = amount;
    }

    /// <summary>
    /// Calculates discount based on quantity following business rules:
    /// - 4-9 items: 10% discount
    /// - 10-20 items: 20% discount
    /// - Below 4 or above 20: No discount (0%)
    /// </summary>
    /// <param name="quantity">Quantity of items</param>
    /// <param name="unitPrice">Unit price of the item</param>
    /// <returns>Discount instance</returns>
    public static Discount Calculate(int quantity, decimal unitPrice)
    {
        decimal percentage = GetDiscountPercentage(quantity);
        decimal totalBeforeDiscount = quantity * unitPrice;
        decimal discountAmount = totalBeforeDiscount * (percentage / 100);

        return new Discount(percentage, discountAmount);
    }

    /// <summary>
    /// Gets the discount percentage based on quantity
    /// </summary>
    /// <param name="quantity">Quantity of items</param>
    /// <returns>Discount percentage</returns>
    public static decimal GetDiscountPercentage(int quantity)
    {
        return quantity switch
        {
            >= 10 and <= 20 => 20m,
            >= 4 and < 10 => 10m,
            _ => 0m
        };
    }

    /// <summary>
    /// Validates if the quantity is allowed for sale
    /// </summary>
    /// <param name="quantity">Quantity to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidQuantity(int quantity)
    {
        return quantity is > 0 and <= 20;
    }

    /// <summary>
    /// Creates a zero discount
    /// </summary>
    public static Discount Zero() => new(0, 0);
}
