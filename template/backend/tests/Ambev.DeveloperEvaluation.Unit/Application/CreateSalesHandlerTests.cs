using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and mocks.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = Sale.Create(
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName
        );

        foreach (var itemDto in command.Items)
        {
            var item = SaleItem.Create(
                itemDto.ProductId,
                itemDto.ProductName,
                itemDto.Quantity,
                itemDto.UnitPrice
            );
            sale.AddItem(item);
        }

        var expectedResult = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount,
            TotalDiscount = sale.TotalDiscount
        };

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id);
        result.SaleNumber.Should().Be(sale.SaleNumber);
        result.CustomerId.Should().Be(command.CustomerId);
        result.CustomerName.Should().Be(command.CustomerName);
        result.BranchId.Should().Be(command.BranchId);
        result.BranchName.Should().Be(command.BranchName);

        await _saleRepository.Received(1).CreateAsync(
            Arg.Any<Sale>(),
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that sale items are properly added to the sale.
    /// </summary>
    [Fact(DisplayName = "Given sale with multiple items When creating sale Then all items are added")]
    public async Task Handle_MultipleItems_AddsAllItems()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandWithItemCount(3);
        Sale? capturedSale = null;

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                capturedSale = callInfo.ArgAt<Sale>(0);
                return capturedSale;
            });

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(callInfo => new CreateSaleResult
            {
                Id = callInfo.ArgAt<Sale>(0).Id
            });

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale!.Items.Should().HaveCount(3);
        capturedSale.TotalAmount.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Tests that sale total amount is calculated correctly with discounts.
    /// </summary>
    [Fact(DisplayName = "Given sale items with quantity 10 When creating sale Then discount is applied")]
    public async Task Handle_ItemsWithDiscount_CalculatesCorrectTotal()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.Items = new List<CreateSaleItemDto>
        {
            CreateSaleHandlerTestData.GenerateSaleItemDtoWithQuantity(10, 100m) // Should get 10% discount
        };

        Sale? capturedSale = null;

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                capturedSale = callInfo.ArgAt<Sale>(0);
                return capturedSale;
            });

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(callInfo => new CreateSaleResult
            {
                Id = callInfo.ArgAt<Sale>(0).Id,
                TotalAmount = callInfo.ArgAt<Sale>(0).TotalAmount,
                TotalDiscount = callInfo.ArgAt<Sale>(0).TotalDiscount
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale!.Items.First().DiscountPercentage.Should().Be(10);
        capturedSale.TotalDiscount.Should().Be(100m); // 10% of 1000
        capturedSale.TotalAmount.Should().Be(900m); // 1000 - 100
    }

    /// <summary>
    /// Tests that sale with invalid item quantity throws exception.
    /// </summary>
    [Fact(DisplayName = "Given sale item with quantity above 20 When creating sale Then throws exception")]
    public async Task Handle_ItemQuantityAbove20_ThrowsException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.Items = new List<CreateSaleItemDto>
        {
            CreateSaleHandlerTestData.GenerateSaleItemDtoWithQuantity(21, 10m)
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Invalid quantity*");
    }

    /// <summary>
    /// Tests that sale with zero or negative unit price throws exception.
    /// </summary>
    [Fact(DisplayName = "Given sale item with zero unit price When creating sale Then throws exception")]
    public async Task Handle_ItemWithZeroPrice_ThrowsException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.Items = new List<CreateSaleItemDto>
        {
            CreateSaleHandlerTestData.GenerateSaleItemDtoWithQuantity(5, 0m)
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Unit price must be greater than zero*");
    }

    /// <summary>
    /// Tests that sale without items throws validation exception.
    /// </summary>
    [Fact(DisplayName = "Given sale with no items When creating sale Then throws validation exception")]
    public async Task Handle_NoItems_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        command.Items = new List<CreateSaleItemDto>();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that sale is created with correct status.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then status is Active")]
    public async Task Handle_ValidRequest_CreatesActiveSale()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        Sale? capturedSale = null;

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                capturedSale = callInfo.ArgAt<Sale>(0);
                return capturedSale;
            });

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(callInfo => new CreateSaleResult
            {
                Id = callInfo.ArgAt<Sale>(0).Id,
                TotalDiscount = callInfo.ArgAt<Sale>(0).TotalDiscount
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale!.Status.Should().Be(SaleStatus.Active);
        capturedSale.IsCancelled.Should().BeFalse();
    }

    /// <summary>
    /// Tests that sale number is generated automatically.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then sale number is generated")]
    public async Task Handle_ValidRequest_GeneratesSaleNumber()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        Sale? capturedSale = null;

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                capturedSale = callInfo.ArgAt<Sale>(0);
                return capturedSale;
            });

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(callInfo => new CreateSaleResult
            {
                Id = callInfo.ArgAt<Sale>(0).Id,
                SaleNumber = callInfo.ArgAt<Sale>(0).SaleNumber
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale!.SaleNumber.Should().NotBeNullOrEmpty();
    }
}