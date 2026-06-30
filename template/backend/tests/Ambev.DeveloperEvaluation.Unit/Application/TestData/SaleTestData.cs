using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides test data generation for Sale-related tests
/// </summary>
public static class SaleTestData
{
    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .CustomInstantiator(f =>
        {
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            return Sale.Create(
                customerId,
                f.Person.FullName,
                branchId,
                $"Branch {f.Address.City()}"
            );
        });

    private static readonly Faker<SaleItem> saleItemFaker = new Faker<SaleItem>()
        .CustomInstantiator(f =>
        {
            var productId = Guid.NewGuid();
            var quantity = f.Random.Int(1, 20);
            var unitPrice = f.Finance.Amount(1, 100);
            return SaleItem.Create(
                productId,
                f.Commerce.ProductName(),
                quantity,
                unitPrice
            );
        });

    public static Sale GenerateValidSale()
    {
        var sale = saleFaker.Generate();
        var items = saleItemFaker.Generate(3);
        foreach (var item in items)
        {
            sale.AddItem(item);
        }
        return sale;
    }

    public static SaleItem GenerateValidSaleItem() => saleItemFaker.Generate();

    public static List<SaleItem> GenerateSaleItemList(int count = 3) => saleItemFaker.Generate(count);
}