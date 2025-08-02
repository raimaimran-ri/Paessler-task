using AutoMapper;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;


namespace Paessler.Task.Services.Mappers
{
    public class ProductOrderedMapper : Profile
    {
        public ProductOrderedMapper()
        {
            CreateMap<ProductOrdered, ProductOrderedDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product != null ? src.Product.id : 0))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.name : string.Empty))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.price : 0f))
                .ForMember(dest => dest.ProductAmount, opt => opt.MapFrom(src => src.Product != null ? src.amount : 0))
            .ReverseMap();

            CreateMap<ProductOrderedDTO, ProductOrdered>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Product = new Product
                    {
                        id = src.ProductId,
                        name = src.ProductName,
                        price = src.ProductPrice
                    };
                    dest.amount = src.ProductAmount;
                });
        }
    }
}