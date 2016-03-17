using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Bootstrappers.Ioc.AutoMapperProfiles
{
    public class LendingMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<Lending, LendingViewModel>()
                         .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LedingId))
                         .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.LendingStatus)); 
            CreateMap<LendingViewModel, Lending>()
                 .ForMember(dest => dest.LedingId, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.LendingStatus, opt => opt.MapFrom(src => src.Status));
        }
    }
}