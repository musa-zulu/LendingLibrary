using AutoMapper;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Controllers;
using NSubstitute;

namespace LendingLibrary.Tests.Common.Builders.Controllers
{
    public class LendingControllerBuilder
    {
        public LendingControllerBuilder()
        {
            ItemsRepository = Substitute.For<IItemsRepository>();
            MappingEngine = Substitute.For<IMappingEngine>();
            PersonRepository = Substitute.For<IPersonRepository>();
            LendingRepository = Substitute.For<ILendingRepository>();
        }

        private IItemsRepository ItemsRepository { get; set; }
        private IMappingEngine MappingEngine { get; set; }
        private IPersonRepository PersonRepository { get; set; }
        private ILendingRepository LendingRepository { get; set; }

        public LendingControllerBuilder WithItemsRepository(IItemsRepository itemsRepository)
        {
            ItemsRepository = itemsRepository;
            return this;
        }

        public LendingControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            MappingEngine = mappingEngine;
            return this;
        }

        public LendingControllerBuilder WithPersonRepository(IPersonRepository personRepository)
        {
            PersonRepository = personRepository;
            return this;
        }

        public LendingControllerBuilder WithLendingRepository(ILendingRepository lendingRepository)
        {
            LendingRepository = lendingRepository;
            return this;
        }

        public LendingController Build()
        {
            return new LendingController(this.LendingRepository, this.MappingEngine, this.PersonRepository, this.ItemsRepository);
        }
    }
}