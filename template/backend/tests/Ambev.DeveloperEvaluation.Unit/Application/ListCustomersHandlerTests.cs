using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="ListCustomersHandler"/> class.
/// </summary>
public class ListCustomersHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ListCustomersHandler _handler;

    public ListCustomersHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListCustomersHandler(_customerRepository, _mapper);
    }

    [Fact(DisplayName = "Given customers exist When listing customers Then returns all customers")]
    public async Task Handle_CustomersExist_ReturnsAllCustomers()
    {
        // Given
        var customers = CustomerTestData.GenerateCustomerList(5);
        var query = new ListCustomersQuery();
        var customerDtos = customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            IsActive = c.IsActive
        }).ToList();

        _customerRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(customers);
        _mapper.Map<List<CustomerDto>>(customers).Returns(customerDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Customers.Should().HaveCount(5);
    }
}