using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Bootstrappers.Ioc.AutoMapperProfiles
{
    public class PersonMapping : Profile
    {
        protected override void Configure()
        {
            CreateMap<Person, PersonViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PersonId));
            CreateMap<PersonViewModel, Person>()
              .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Id));
        }
    }
}