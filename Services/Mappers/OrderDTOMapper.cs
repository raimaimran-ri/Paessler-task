using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;


namespace Paessler.Task.Services.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {

            CreateMap<ProductOrderedDTO, ProductOrdered>().ReverseMap();
            CreateMap<Order, OrderDTO>()

                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.created_at))
                .ForMember(dest => dest.InvoiceAddress, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.address : string.Empty))
                .ForMember(dest => dest.InvoiceEmailAddress, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.email : string.Empty))
                .ForMember(dest => dest.InvoiceCreditCardNumber, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.credit_card_number : string.Empty))
                .ForMember(dest => dest.ProductOrdered, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.ProductOrdered = src.ProductOrdered != null
                        ? context.Mapper.Map<List<ProductOrderedDTO>>(src.ProductOrdered)
                        : new List<ProductOrderedDTO>();
                })
            .ReverseMap();

            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.created_at, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.ProductOrdered, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    if (src.ProductOrdered != null)
                    {
                        dest.ProductOrdered = context.Mapper.Map<List<ProductOrdered>>(src.ProductOrdered);
                    }
                    else
                    {
                        dest.ProductOrdered = new List<ProductOrdered>();
                    }
                })
                .AfterMap((src, dest) =>
                {
                    if (dest.Customer == null)
                    {
                        dest.Customer = new Customer();
                    }
                    dest.Customer.address = src.InvoiceAddress;
                    dest.Customer.email = src.InvoiceEmailAddress;
                    dest.Customer.credit_card_number = src.InvoiceCreditCardNumber;
                });
        }
    }
}