using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransactionModel, TransactionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BeneficiaryName, opt => opt.MapFrom(src => src.BeneficiaryName))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.MCC, opt => opt.MapFrom(src => src.MCC))
                .ForMember(dest => dest.Kind, opt => opt.MapFrom(src => src.Kind))
                .ForMember(dest => dest.CategoryDto, opt => opt.MapFrom(src => src.CategoryModel))
                .ForMember(dest => dest.SplitsDto, opt => opt.MapFrom(src => src.Splits));
        }
    }
}
