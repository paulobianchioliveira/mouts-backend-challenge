using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;

public class DeleteCustomerCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}