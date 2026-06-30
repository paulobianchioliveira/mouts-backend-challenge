using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CustomerTestData
{
    public static List<Customer> GenerateCustomerList(int count)
    {
        var customers = new List<Customer>();
        for (int i = 0; i < count; i++)
        {
            customers.Add(GenerateValidCustomer(i));
        }
        return customers;
    }

    public static CreateCustomerCommand GenerateValidCreateCommand()
    {
        return new CreateCustomerCommand
        {
            Name = "Test Customer",
            Email = "test.customer@example.com"
        };
    }

    public static Customer GenerateValidCustomer(int? id = null)
    {
        return new Customer
        {
            Id = id.HasValue ? new Guid(id.Value, 0, 0, new byte[8]) : Guid.NewGuid(),
            Name = "Test Customer",
            Email = "test.customer@example.com"
        };
    }
}