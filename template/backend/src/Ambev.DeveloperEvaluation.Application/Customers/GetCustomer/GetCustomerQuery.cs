using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerQuery : IRequest<GetCustomerResult>
{
    public Guid Id { get; set; }
}