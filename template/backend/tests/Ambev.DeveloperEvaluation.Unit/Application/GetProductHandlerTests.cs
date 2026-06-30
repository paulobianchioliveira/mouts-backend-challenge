using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetProductHandler"/> class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductHandler _handler;

    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid product ID When getting product Then returns product")]
    public async Task Handle_ValidId_ReturnsProduct()
    {
        // Given
        var product = ProductTestData.GenerateValidProduct();
        var query = new GetProductQuery { Id = product.Id };
        var result = new GetProductResult
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.UnitPrice,
            Category = product.Category
        };

        _productRepository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(result);

        // When
        var getProductResult = await _handler.Handle(query, CancellationToken.None);

        // Then
        getProductResult.Should().NotBeNull();
        getProductResult.Id.Should().Be(product.Id);
        getProductResult.Name.Should().Be(product.Name);
    }

    [Fact(DisplayName = "Given non-existent product ID When getting product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentId_ThrowsKeyNotFoundException()
    {
        // Given
        var query = new GetProductQuery { Id = Guid.NewGuid() };
        _productRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // When
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}