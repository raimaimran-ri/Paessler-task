using AutoMapper;
using Paessler.Task.Model.Models;
using Paessler.Task.Services.DTOs;


namespace Paessler.Task.Services.Mappers
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.InvoiceEmailAddress, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.InvoiceAddress, opt => opt.MapFrom(src => src.address))
                .ForMember(dest => dest.InvoiceCreditCardNumber, opt => opt.MapFrom(src => src.credit_card_number))
            .ReverseMap();
        }
    }
}