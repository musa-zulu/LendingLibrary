using System.Collections.Generic;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Bootstrappers.Ioc.AutoMapperProfiles
{
    public class ItemsMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<List<Item>, List<ItemViewModel>>();
            CreateMap<List<ItemViewModel>, List<Item>>();

            CreateMap<Item, ItemViewModel>();
            CreateMap<ItemViewModel, Item>();
        }
    }
}