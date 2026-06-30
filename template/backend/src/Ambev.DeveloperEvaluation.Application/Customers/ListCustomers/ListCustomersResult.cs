namespace Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

public class ListCustomersResult
{
    public List<CustomerDto> Customers { get; set; } = new();
}

public class CustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}