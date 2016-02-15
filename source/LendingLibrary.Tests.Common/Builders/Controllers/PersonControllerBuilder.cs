using AutoMapper;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Controllers;
using NSubstitute;

namespace LendingLibrary.Tests.Common.Builders.Controllers
{
    public class PersonControllerBuilder
    {
        public PersonControllerBuilder()
        {
            PersonRepository = Substitute.For<IPersonRepository>();
            MappingEngine = Substitute.For<IMappingEngine>();
        }

        public IPersonRepository PersonRepository { get; private set; }
        public IMappingEngine MappingEngine { get; private set; }

        public PersonControllerBuilder WithPersonRepository(IPersonRepository personRepository)
        {
            PersonRepository = personRepository;
            return this;
        }

        public PersonControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            MappingEngine = mappingEngine;
            return this;
        }

        public PersonController Build()
        {
            return new PersonController(PersonRepository, MappingEngine);
        }
    }
}