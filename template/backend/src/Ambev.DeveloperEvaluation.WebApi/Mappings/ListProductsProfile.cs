using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    /// <summary>
    /// Profile for mapping between Product entity and ProductDto
    /// </summary>
    public class ListProductsProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for ListProducts operation
        /// </summary>
        public ListProductsProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Stock, opt => opt.Ignore()); // Stock property doesn't exist on Product entity
        }
    }
}
