using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateSaleHandler tests.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand entities.
    /// The generated commands will have valid:
    /// - CustomerId and CustomerName
    /// - BranchId and BranchName
    /// - List of sale items with valid quantities (1-20) and prices
    /// </summary>
    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerName, f => f.Person.FullName)
        .RuleFor(s => s.BranchId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchName, f => $"Branch {f.Address.City()}")
        .RuleFor(s => s.Items, f => GenerateSaleItems(f.Random.Int(1, 5)));

    /// <summary>
    /// Configures the Faker to generate valid CreateSaleItemDto entities.
    /// </summary>
    private static readonly Faker<CreateSaleItemDto> createSaleItemDtoFaker = new Faker<CreateSaleItemDto>()
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 100));

    /// <summary>
    /// Generates a valid CreateSaleCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CreateSaleCommand with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateSaleCommand with a specific number of items.
    /// </summary>
    /// <param name="itemCount">Number of items to include in the sale</param>
    /// <returns>A valid CreateSaleCommand with the specified number of items.</returns>
    public static CreateSaleCommand GenerateCommandWithItemCount(int itemCount)
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = GenerateSaleItems(itemCount);
        return command;
    }

    /// <summary>
    /// Generates a list of valid CreateSaleItemDto entities.
    /// </summary>
    /// <param name="count">Number of items to generate</param>
    /// <returns>List of CreateSaleItemDto with valid data.</returns>
    public static List<CreateSaleItemDto> GenerateSaleItems(int count = 3)
    {
        return createSaleItemDtoFaker.Generate(count);
    }

    /// <summary>
    /// Generates a valid CreateSaleItemDto with randomized data.
    /// </summary>
    /// <returns>A valid CreateSaleItemDto with randomly generated data.</returns>
    public static CreateSaleItemDto GenerateValidSaleItemDto()
    {
        return createSaleItemDtoFaker.Generate();
    }

    /// <summary>
    /// Generates a CreateSaleItemDto with a specific quantity for testing discount rules.
    /// </summary>
    /// <param name="quantity">Quantity to set (1-20)</param>
    /// <param name="unitPrice">Unit price for the item</param>
    /// <returns>A CreateSaleItemDto with the specified quantity.</returns>
    public static CreateSaleItemDto GenerateSaleItemDtoWithQuantity(int quantity, decimal unitPrice = 10m)
    {
        var item = createSaleItemDtoFaker.Generate();
        item.Quantity = quantity;
        item.UnitPrice = unitPrice;
        return item;
    }
}