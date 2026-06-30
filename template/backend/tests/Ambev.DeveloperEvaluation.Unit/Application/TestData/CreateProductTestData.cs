using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides test data generation for Product-related tests
/// </summary>
public static class ProductTestData
{
    private static readonly Faker<CreateProductCommand> createProductFaker = new Faker<CreateProductCommand>()
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Finance.Amount(1, 1000))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Stock, f => f.Random.Int(0, 500));

    private static readonly Faker<UpdateProductCommand> updateProductFaker = new Faker<UpdateProductCommand>()
        .RuleFor(p => p.Id, f => Guid.NewGuid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => f.Finance.Amount(1, 1000))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.Stock, f => f.Random.Int(0, 500));

    private static readonly Faker<Product> productEntityFaker = new Faker<Product>()
        .RuleFor(p => p.Id, f => Guid.NewGuid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.UnitPrice, f => f.Finance.Amount(1, 1000))
        .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
        .RuleFor(p => p.IsActive, f => true);

    public static CreateProductCommand GenerateValidCreateCommand() => createProductFaker.Generate();

    public static UpdateProductCommand GenerateValidUpdateCommand() => updateProductFaker.Generate();

    public static Product GenerateValidProduct() => productEntityFaker.Generate();

    public static List<Product> GenerateProductList(int count = 5) => productEntityFaker.Generate(count);
}