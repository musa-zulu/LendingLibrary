using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using Castle.Windsor;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Tests.Common.Builders.Controllers;
using LendingLibrary.Tests.Common.Builders.Domain;
using LendingLibrary.Tests.Common.Builders.ViewModels;
using LendingLibrary.Tests.Common.Helpers;
using LendingLibrary.Web.Bootstrappers.Installers;
using LendingLibrary.Web.Controllers;
using LendingLibrary.Web.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestItemsController
    {

        private IWindsorContainer _container;
        private readonly WindsorTestHelpers _windsorTestHelpers = new WindsorTestHelpers();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _container = _windsorTestHelpers.CreateContainerWith(new AutoMapperInstaller());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            if (_container != null) _container.Dispose();
        }

        [Test]
        public void Contruct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new ItemsController(Substitute.For<IItemsRepository>(), Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenIItemsRepositoryIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new ItemsController(null, Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("itemsRepository", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var itemsController = CreateItemsControllerBuilder()
                                 .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = (ViewResult)itemsController.Index();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Index_ShouldCallGetAll()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();

            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsController.Index();
            //---------------Test Result -----------------------
            itemsRepository.Received(1).GetAllItems();
        }

        [Test]
        public void Index_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var items = new List<Item>();
            itemsRepository.GetAllItems().Returns(items);

            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsController.Index();
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<List<Item>, List<ItemViewModel>>(items);
        }

        [Test]
        public void Index_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var items = new List<Item>();
            itemsRepository.GetAllItems().Returns(items);

            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = (ViewResult)itemsController.Index();
            //---------------Test Result -----------------------
            var model = result.Model;
            Assert.IsInstanceOf<List<ItemViewModel>>(model);
        }

        [Test]
        public void Create_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();

            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = (ViewResult)itemsController.Create();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemsViewModel = new ItemViewModel();

            var itemsController = CreateItemsControllerBuilder()
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Create(itemsViewModel) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<ItemViewModel, Item>(itemsViewModel);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallSaveFromItemsRepo()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var itemViewModel = ItemsViewModelBuilder.BuildRandom();

            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Create(itemViewModel);
            //---------------Test Result -----------------------
            itemsRepository.Received(1).Save(Arg.Any<Item>());
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldRedirectToItemsIndexPage()
        {
            //---------------Set up test pack-------------------
            var itemsViewModel = new ItemViewModel();

            var itemsController = CreateItemsControllerBuilder().Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Create(itemsViewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Items", result.RouteValues["Controller"]);
        }

        [Test]
        public void Create_POST_GivenModelStateIsInvalid_ShouldReturnViewWithItemsViewModel()
        {
            //---------------Set up test pack-------------------
            var itemsViewModel = new ItemViewModel();
            var itemsController = CreateItemsControllerBuilder().Build();

            itemsController.ModelState.AddModelError("key", "some error");
            //---------------Assert Precondition----------------
            Assert.IsFalse(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Create(itemsViewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.AreEqual(itemsViewModel, model);
        }

        [Test]
        public void Create_POST_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ItemsController).GetMethod("Create", new[] { typeof(ItemViewModel) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public void Edit_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var itemsController = CreateItemsControllerBuilder().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.Edit(Guid.Empty);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Edit_GivenItemId_ShouldCallGetByIdFromTheItemsRepo()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var item = ItemBuilder.BuildRandom();
            var id = item.Id;
            itemsRepository.GetById(id).Returns(item);

            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            itemsRepository.Received(1).GetById(id);
        }

        [Test]
        public void Edit_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var item = ItemBuilder.BuildRandom();
            var id = item.Id;
            itemsRepository.GetById(id).Returns(item);

            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.Edit(id);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<Item, ItemViewModel>(item);
        }

        [Test]
        public void Edit_ShouldReturnViewWithItemViewModel()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var item = new ItemBuilder().WithRandomProps().Build();
            var id = item.Id;
            itemsRepository.GetById(id).Returns(item);

            var itemsController = CreateItemsControllerBuilder()
                .WithMappingEngine(mappingEngine)
                .WithItemsRepository(itemsRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsNotNull(model);
            Assert.IsInstanceOf<ItemViewModel>(model);
        }

        [Test]
        public void Edit_POST_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ItemsController).GetMethod("Edit", new[] { typeof(ItemViewModel) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var itemViewModel = ItemsViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();

            var itemsController = CreateItemsControllerBuilder()
                                .WithMappingEngine(mappingEngine)
                                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Edit(itemViewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<ItemViewModel, Item>(itemViewModel);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallSaveFromItemsRepo()
        {
            //---------------Set up test pack-------------------
            var itemViewModel = ItemsViewModelBuilder.BuildRandom();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();

            var itemsController = CreateItemsControllerBuilder()
                                .WithItemsRepository(itemsRepository)
                                .WithMappingEngine(mappingEngine)
                                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Edit(itemViewModel);
            //---------------Test Result -----------------------
            itemsRepository.Received(1).Save(Arg.Any<Item>());
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldRedirectToItemsIndexPage()
        {
            //---------------Set up test pack-------------------
            var itemViewModel = ItemsViewModelBuilder.BuildRandom();

            var itemsController = CreateItemsControllerBuilder()
                                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Edit(itemViewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Items", result.RouteValues["Controller"]);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsInvalid_ShouldReturnViewWithViewItemsViewModel()
        {
            //---------------Set up test pack-------------------
            var itemViewModel = ItemsViewModelBuilder.BuildRandom();

            var itemsController = CreateItemsControllerBuilder().Build();
            itemsController.ModelState.AddModelError("key", "error message");
            //---------------Assert Precondition----------------
            Assert.IsFalse(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Edit(itemViewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsNotNull(model);
            Assert.IsInstanceOf<ItemViewModel>(model);
        }

        [Test]
        public void Delete_GivenItemIdIsEmpty_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var itemsControllerBuilder = CreateItemsControllerBuilder()
                                        .WithItemsRepository(itemsRepository)
                                        .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsControllerBuilder.Delete(Guid.Empty);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Delete_GivenAValidId_ShouldCallGetById()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var itemsControllerBuilder = CreateItemsControllerBuilder()
                                        .WithItemsRepository(itemsRepository)
                                        .Build();
            var item = ItemBuilder.BuildRandom();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            itemsControllerBuilder.Delete(item.Id);
            //---------------Test Result -----------------------
            itemsRepository.Received(1).GetById(item.Id);
        }
        

        [Test]
        public void Delete_GivenAValidId_ShouldCallDeleteItem()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var item = ItemBuilder.BuildRandom();
            itemsRepository.GetById(item.Id).Returns(item);
            var itemsControllerBuilder = CreateItemsControllerBuilder()
                                        .WithItemsRepository(itemsRepository)
                                        .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            itemsControllerBuilder.Delete(item.Id);
            //---------------Test Result -----------------------
            itemsRepository.Received(1).DeleteItem(item);
        }
        
        private static ItemsControllerBuilder CreateItemsControllerBuilder()
        {
            return new ItemsControllerBuilder();
        }
    }
}