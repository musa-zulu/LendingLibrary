using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Bootstrappers.Ioc.AutoMapperProfiles
{
    public class ItemsMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<Item, ItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId));
            CreateMap<ItemViewModel, Item>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id));
        }
    }
}