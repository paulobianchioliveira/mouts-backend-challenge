using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="Sale"/> entity.
/// </summary>
public class SaleTests
{
    [Fact(DisplayName = "Given valid data When creating sale Then sale is created successfully")]
    public void Create_ValidData_ReturnsSale()
    {
        // Given
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var branchId = Guid.NewGuid();
        var branchName = "Branch 1";

        // When
        var sale = Sale.Create(customerId, customerName, branchId, branchName);

        // Then
        sale.Should().NotBeNull();
        sale.CustomerId.Should().Be(customerId);
        sale.CustomerName.Should().Be(customerName);
        sale.BranchId.Should().Be(branchId);
        sale.BranchName.Should().Be(branchName);
        sale.Status.Should().Be(SaleStatus.Active);
        sale.TotalAmount.Should().Be(0);
    }

    [Fact(DisplayName = "Given sale item When adding item Then total is calculated")]
    public void AddItem_ValidItem_UpdatesTotal()
    {
        // Given
        var sale = SaleTestData.GenerateValidSale();
        var initialTotal = sale.TotalAmount;
        var item = SaleTestData.GenerateValidSaleItem();

        // When
        sale.AddItem(item);

        // Then
        sale.Items.Should().Contain(item);
        sale.TotalAmount.Should().BeGreaterThan(initialTotal);
    }

    [Fact(DisplayName = "Given cancelled sale When adding item Then throws exception")]
    public void AddItem_CancelledSale_ThrowsException()
    {
        // Given
        var sale = SaleTestData.GenerateValidSale();
        sale.Cancel("Test cancellation");
        var item = SaleTestData.GenerateValidSaleItem();

        // When
        var act = () => sale.AddItem(item);

        // Then
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add items to a cancelled sale.");
    }

    [Fact(DisplayName = "Given active sale When cancelling Then sale is cancelled")]
    public void Cancel_ActiveSale_CancelsSale()
    {
        // Given
        var sale = SaleTestData.GenerateValidSale();
        var reason = "Customer request";

        // When
        sale.Cancel(reason);

        // Then
        sale.IsCancelled.Should().BeTrue();
        sale.CancellationReason.Should().Be(reason);
        sale.CancelledAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given quantity 5 When creating sale item Then 10% discount is applied")]
    public void CreateSaleItem_Quantity5_Applies10PercentDiscount()
    {
        // Given
        var productId = Guid.NewGuid();
        var productName = "Product Test";
        var quantity = 5;
        var unitPrice = 10m;

        // When
        var item = SaleItem.Create(productId, productName, quantity, unitPrice);

        // Then
        item.Quantity.Should().Be(quantity);
        item.UnitPrice.Should().Be(unitPrice);
        item.DiscountPercentage.Should().Be(10);
        item.DiscountAmount.Should().Be(5m); // 10% of 50
        item.TotalAmount.Should().Be(45m); // 50 - 5
    }

    [Fact(DisplayName = "Given quantity 15 When creating sale item Then 20% discount is applied")]
    public void CreateSaleItem_Quantity15_Applies20PercentDiscount()
    {
        // Given
        var productId = Guid.NewGuid();
        var productName = "Product Test";
        var quantity = 15;
        var unitPrice = 10m;

        // When
        var item = SaleItem.Create(productId, productName, quantity, unitPrice);

        // Then
        item.Quantity.Should().Be(quantity);
        item.DiscountPercentage.Should().Be(20);
        item.DiscountAmount.Should().Be(30m); // 20% of 150
        item.TotalAmount.Should().Be(120m); // 150 - 30
    }

    [Fact(DisplayName = "Given quantity above 20 When creating sale item Then throws exception")]
    public void CreateSaleItem_QuantityAbove20_ThrowsException()
    {
        // Given
        var productId = Guid.NewGuid();
        var productName = "Product Test";
        var quantity = 21;
        var unitPrice = 10m;

        // When
        var act = () => SaleItem.Create(productId, productName, quantity, unitPrice);

        // Then
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid quantity: 21. Must be between 1 and 20.*");
    }
}