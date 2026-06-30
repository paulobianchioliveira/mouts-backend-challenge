using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;

namespace Ambev.DeveloperEvaluation.Application.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<Customer, CreateCustomerResult>();

        CreateMap<UpdateCustomerCommand, Customer>();
        CreateMap<Customer, UpdateCustomerResult>();

        CreateMap<Customer, GetCustomerResult>();

        CreateMap<Customer, CustomerDto>();
    }
}