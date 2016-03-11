using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Bootstrappers.Ioc.AutoMapperProfiles
{
    public class LendingMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<Lending, LendingViewModel>();
            CreateMap<LendingViewModel, Lending>();
        }
    }
}