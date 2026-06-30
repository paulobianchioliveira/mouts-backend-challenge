using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="ListProductsHandler"/> class.
/// </summary>
public class ListProductsHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ListProductsHandler _handler;

    public ListProductsHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListProductsHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given products exist When listing products Then returns all products")]
    public async Task Handle_ProductsExist_ReturnsAllProducts()
    {
        // Given
        var products = ProductTestData.GenerateProductList(5);
        var query = new ListProductsQuery();
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.UnitPrice,
            Category = p.Category
        }).ToList();

        _productRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(products);
        _mapper.Map<List<ProductDto>>(products).Returns(productDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Products.Should().HaveCount(5);
    }

    [Fact(DisplayName = "Given no products exist When listing products Then returns empty list")]
    public async Task Handle_NoProducts_ReturnsEmptyList()
    {
        // Given
        var query = new ListProductsQuery();
        _productRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Product>());
        _mapper.Map<List<ProductDto>>(Arg.Any<List<Product>>())
            .Returns(new List<ProductDto>());

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Products.Should().BeEmpty();
    }
}