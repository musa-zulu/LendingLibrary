using System.Collections.Generic;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.ViewModels;

namespace LendingLibrary.Web.Bootstrappers.Ioc.AutoMapperProfiles
{
    public class PersonMapping : Profile
    {
        protected override void Configure()
        {
            CreateMap<List<Person>, List<PersonViewModel>>();
            CreateMap<List<PersonViewModel>, List<Person>>();

            CreateMap<Person, PersonViewModel>();
            CreateMap<PersonViewModel, Person>();
        }
    }
}