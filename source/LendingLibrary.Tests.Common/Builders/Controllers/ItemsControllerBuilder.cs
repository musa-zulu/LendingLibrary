using AutoMapper;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Controllers;
using NSubstitute;

namespace LendingLibrary.Tests.Common.Builders.Controllers
{
    public class ItemsControllerBuilder
    {
        public ItemsControllerBuilder()
        {
            ItemsRepository = Substitute.For<IItemsRepository>();
            MappingEngine = Substitute.For<IMappingEngine>();
        }

        public IItemsRepository ItemsRepository { get; private set; }
        public IMappingEngine MappingEngine { get; private set; }

        public ItemsControllerBuilder WithItemsRepository(IItemsRepository itemsRepository)
        {
            ItemsRepository = itemsRepository;
            return this;
        }

        public ItemsControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            MappingEngine = mappingEngine;
            return this;
        }

        public ItemsController Build()
        {
            return new ItemsController(ItemsRepository, MappingEngine);
        }
    }
}