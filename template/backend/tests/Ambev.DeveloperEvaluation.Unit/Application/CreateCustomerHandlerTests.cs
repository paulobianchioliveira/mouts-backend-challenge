using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateCustomerHandler"/> class.
/// </summary>
public class CreateCustomerHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateCustomerHandler(_customerRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid customer data When creating customer Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CustomerTestData.GenerateValidCreateCommand();
        var customer = CustomerTestData.GenerateValidCustomer();
        var result = new CreateCustomerResult
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };

        _mapper.Map<Customer>(command).Returns(customer);
        _mapper.Map<CreateCustomerResult>(customer).Returns(result);
        _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(customer);

        // When
        var createCustomerResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createCustomerResult.Should().NotBeNull();
        createCustomerResult.Id.Should().Be(customer.Id);
        createCustomerResult.Name.Should().Be(customer.Name);
        await _customerRepository.Received(1).CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
    }
}