using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using Castle.Windsor;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
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
    public class TestLendingController
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
        public void Construct()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new LendingController(Substitute.For<ILendingRepository>(), Substitute.For<IMappingEngine>(), Substitute.For<IPersonRepository>(), Substitute.For<IItemsRepository>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenILendingRepositoryIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new LendingController(null, Substitute.For<IMappingEngine>(), Substitute.For<IPersonRepository>(), Substitute.For<IItemsRepository>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("lendingRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMappingEngineIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new LendingController(Substitute.For<ILendingRepository>(), null,  Substitute.For<IPersonRepository>(), Substitute.For<IItemsRepository>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("mappingEngine", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIItemsRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(
                    () =>
                        new LendingController(Substitute.For<ILendingRepository>(), Substitute.For<IMappingEngine>(),
                            Substitute.For<IPersonRepository>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("itemsRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIPersonRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(()=> new LendingController(Substitute.For<ILendingRepository>(), Substitute.For<IMappingEngine>(), null, Substitute.For<IItemsRepository>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("personRepository", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = (ViewResult)controller.Index();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            var model = results.Model;
            Assert.IsInstanceOf<List<LendingViewModel>>(model);
        }

        [Test]
        public void Index_ShouldCallGetAll()
        {
            //---------------Set up test pack-------------------
            var lendingRepository = Substitute.For<ILendingRepository>();
            var controller = CreateLendingController(lendingRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Index();
            //---------------Test Result -----------------------
            lendingRepository.Received().GetAll();
        }

        [Test]
        public void Index_GivenLendingItemsAreReturnedFromDataBase_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var lendingItem = new LendingBuilder().WithRandomGeneratedId().WithRandomProps().Build();
            var lendingRepository = Substitute.For<ILendingRepository>();
            var lendings = new List<Lending> { lendingItem };
            lendingRepository.GetAll().Returns(lendings);
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateLendingController(lendingRepository, mappingEngine);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Index();
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<List<Lending>, List<LendingViewModel>>(lendings);
        }

        [Test]
        public void Create_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Create();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
        }

        [Test]
        public void Create_POST_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(LendingController).GetMethod("Create", new[] { typeof(LendingViewModel) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public void Create_POST_ShouldHaveValidAntiForgeryTokenAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(LendingController).GetMethod("Create", new[] { typeof(LendingViewModel) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var validAntityForgeryTokenAttribute = methodInfo.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(validAntityForgeryTokenAttribute);
        }
        /* 
            [Test]
            public void Create_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
            {
                //---------------Set up test pack-------------------
                var mappingEngine = Substitute.For<IMappingEngine>();
                var itemRepository = Substitute.For<IItemsRepository>();
                var viewModel = LendingViewModelBuilder.BuildRandom();
                itemRepository.GetById(viewModel.ItemId).Returns(Arg.Any<Item>());
                var controller = CreateLendingController(null, mappingEngine);
                //---------------Assert Precondition----------------
                Assert.IsTrue(controller.ModelState.IsValid);
                //---------------Execute Test ----------------------
                var result = controller.Create(viewModel);
                //---------------Test Result -----------------------
                mappingEngine.Received(1).Map<LendingViewModel, Lending>(viewModel);
            }

               [Test]
                    public void Create_POST_GivenModelStateIsValid_ShouldCallSaveOnPersonRepo()
                    {
                        //---------------Set up test pack-------------------
                        var viewModel = LendingViewModelBuilder.BuildRandom();
                        var mappingEngine = _container.Resolve<IMappingEngine>();
                        var lendingRepository = Substitute.For<ILendingRepository>();
                        var controller = CreateLendingController(lendingRepository, mappingEngine);

                        //---------------Assert Precondition----------------
                        Assert.IsTrue(controller.ModelState.IsValid);
                        //---------------Execute Test ----------------------
                        var result = controller.Create(viewModel);
                        //---------------Test Result -----------------------
                        lendingRepository.Received(1).Save(Arg.Any<Lending>());
                    }

                    [Test]
                    public void Create_POST_GivenModelStateIsValid_ShouldRedirectToIndexPage()
                    {
                        //---------------Set up test pack-------------------
                        var viewModel = LendingViewModelBuilder.BuildRandom();
                        var mappingEngine = _container.Resolve<IMappingEngine>();
                        var lendingRepository = Substitute.For<ILendingRepository>();
                        var controller = CreateLendingController(lendingRepository, mappingEngine);

                        //---------------Assert Precondition----------------
                        Assert.IsTrue(controller.ModelState.IsValid);
                        //---------------Execute Test ----------------------
                        var result = controller.Create(viewModel) as RedirectToRouteResult;
                        //---------------Test Result -----------------------
                        Assert.IsNotNull(result);
                        Assert.AreEqual("Index", result.RouteValues["Action"]);
                    }*/

        [Test]
        public void Create_POST_GivenModelStateIsInvalid_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingViewModelBuilder.BuildRandom();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var lendingRepository = Substitute.For<ILendingRepository>();
            var controller = CreateLendingController(lendingRepository, mappingEngine);

            controller.ModelState.AddModelError("key", "some error");

            //---------------Assert Precondition----------------
            Assert.IsFalse(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = (ViewResult)controller.Create(viewModel);
            //---------------Test Result -----------------------
            var model = result.Model;
            Assert.IsInstanceOf<LendingViewModel>(model);
        }

        [Test]
        public void Edit_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var itemsController = CreateLendingController();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.Edit(Guid.Empty);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Edit_GivenIdIsNull_ShouldReturnReturnBadRequest()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit(Guid.Empty) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void Edit_GivenItemId_ShouldCallGetByIdFromTheLendingsRepo()
        {
            //---------------Set up test pack-------------------
            var lending = new LendingBuilder().WithRandomGeneratedId().Build();
            var id = lending.LedingId;
            var lendingRepository = Substitute.For<ILendingRepository>();
            var controller = CreateLendingController(lendingRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            lendingRepository.Received(1).GetById(id);
        }

        [Test]
        public void Edit_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var lending = new LendingBuilder().WithRandomGeneratedId().Build();
            var id = lending.LedingId;
            var mappingEngine = Substitute.For<IMappingEngine>();
            var lendingRepository = Substitute.For<ILendingRepository>();
            lendingRepository.GetById(id).Returns(lending);
            var controller = CreateLendingController(lendingRepository, mappingEngine);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit(id);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<Lending, LendingViewModel>(lending);
        }

        [Test]
        public void Edit_GivenViewModelIsNull_ShouldReturnHttpNotFoundStatus()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit((Guid?)null) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void Edit_ShouldReturnViewWithItemViewModel()
        {
            //---------------Set up test pack-------------------
            var lending = new LendingBuilder().WithRandomGeneratedId().Build();
            var id = lending.LedingId;
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var lendingRepository = Substitute.For<ILendingRepository>();
            lendingRepository.GetById(id).Returns(lending);
            var controller = CreateLendingController(lendingRepository, mappingEngine);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsNotNull(model);
            Assert.IsInstanceOf<LendingViewModel>(model);
        }

        [Test]
        public void Edit_POST_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(LendingController).GetMethod("Edit", new[] { typeof(LendingViewModel) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public void Edit_POST_ShouldHaveValidateAntiForgeryTokenAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(LendingController).GetMethod("Edit", new[] { typeof(LendingViewModel) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var antiForgeryAttribute = methodInfo.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(antiForgeryAttribute);
        }

        /*      [Test]
           public void Edit_POST_GivenModelStateIsValid_ShouldCallGetByIdFromOrderRepo()
           {
               //---------------Set up test pack-------------------
               var lendingsViewModel = new LendingViewModelBuilder().WithRandomProps().Build();
               var mappingEngine = _container.Resolve<IMappingEngine>();
               var lendingRepository = Substitute.For<ILendingRepository>();

               var controller = CreateLendingController(lendingRepository, mappingEngine);

               //---------------Assert Precondition----------------
               Assert.IsTrue(controller.ModelState.IsValid);
               //---------------Execute Test ----------------------
               var result = controller.Edit(lendingsViewModel);
               //---------------Test Result -----------------------
               lendingRepository.Received().GetById(lendingsViewModel.Id);
           }

        [Test]
           public void Edit_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
           {
               //---------------Set up test pack-------------------
               var itemViewModel = new ItemsViewModelBuilder().WithRandomProps().Build();
               var mappingEngine = Substitute.For<IMappingEngine>();

               var itemsController = CreateItemsControllerBuilder()
                                   .WithMappingEngine(mappingEngine)
                                   .Build();
               //---------------Assert Precondition----------------
               Assert.IsTrue(itemsController.ModelState.IsValid);
               //---------------Execute Test ----------------------
               var result = itemsController.Edit(itemViewModel);
               //---------------Test Result -----------------------
               mappingEngine.Received().Map<ItemViewModel, Item>(itemViewModel);
           }

           [Test]
           public void Edit_POST_GivenModelStateIsValid_ShouldCallUpdateFromItemsRepo()
           {
               //---------------Set up test pack-------------------
               var item = new ItemBuilder().WithRandomProps().Build();
               var itemViewModel = ItemsViewModelBuilder.BuildRandom();
               var itemsRepository = Substitute.For<IItemsRepository>();
               itemsRepository.GetById(item.ItemId).Returns(item);
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
               itemsRepository.Received().Update(Arg.Any<Item>(), Arg.Any<Item>());
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
           public void Delete_GivenItemIdIsNull_ShouldReturnHttpStatusOfBadRequest()
           {
               //---------------Set up test pack-------------------
               var itemsRepository = Substitute.For<IItemsRepository>();
               var itemsControllerBuilder = CreateItemsControllerBuilder()
                                           .WithItemsRepository(itemsRepository)
                                           .Build();
               //---------------Assert Precondition----------------
               //---------------Execute Test ----------------------
               var result = itemsControllerBuilder.Delete((Guid?)null) as HttpStatusCodeResult;
               //---------------Test Result -----------------------
               Assert.IsNotNull(result);
               Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
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
               itemsControllerBuilder.Delete(item.ItemId);
               //---------------Test Result -----------------------
               itemsRepository.Received(1).GetById(item.ItemId);
           }

           [Test]
           public void Delete_GivenAnItemIsReturnedFromRepo_ShouldcallMappingEngine()
           {
               //---------------Set up test pack-------------------
               var mappingEngine = Substitute.For<IMappingEngine>();
               var itemsRepository = Substitute.For<IItemsRepository>();
               var item = ItemBuilder.BuildRandom();
               itemsRepository.GetById(item.ItemId).Returns(item);
               var itemsControllerBuilder = CreateItemsControllerBuilder()
                                           .WithMappingEngine(mappingEngine)
                                           .WithItemsRepository(itemsRepository)
                                           .Build();
               //---------------Assert Precondition----------------

               //---------------Execute Test ----------------------
               itemsControllerBuilder.Delete(item.ItemId);
               //---------------Test Result -----------------------
               mappingEngine.Received(1).Map<Item, ItemViewModel>(item);
           }

           [Test]
           public void Delete_GivenItemsViewModelIsNull_ShouldReturnHttpNotFound()
           {
               //---------------Set up test pack-------------------
               var itemsController = CreateItemsControllerBuilder().Build();
               //---------------Assert Precondition----------------

               //---------------Execute Test ----------------------
               var result = itemsController.Delete(Guid.NewGuid()) as HttpStatusCodeResult;
               //---------------Test Result -----------------------
               Assert.IsNotNull(result);
               Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
           }

           [Test]
           public void Delete_ShouldReturnViewWithItemsViewModel()
           {
               //---------------Set up test pack-------------------
               var item = new ItemBuilder().WithRandomProps().Build();
               var mappingEngine = _container.Resolve<IMappingEngine>();
               var itemsRepository = Substitute.For<IItemsRepository>();
               itemsRepository.GetById(item.ItemId).Returns(item);
               var itemsController = CreateItemsControllerBuilder()
                   .WithItemsRepository(itemsRepository)
                   .WithMappingEngine(mappingEngine)
                   .Build();
               //---------------Assert Precondition----------------

               //---------------Execute Test ----------------------
               var result = itemsController.Delete(item.ItemId) as ViewResult;
               //---------------Test Result -----------------------
               Assert.IsNotNull(result);
               var model = result.Model;
               Assert.IsInstanceOf<ItemViewModel>(model);
           }
   */
        [Test]
        public void DeleteConfirmed_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ItemsController).GetMethod("DeleteConfirmed", new[] { typeof(Guid) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public void DeleteConfirmed_ShoulHaveActionNameAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(ItemsController).GetMethod("DeleteConfirmed", new[] { typeof(Guid) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var actionNameAttribute = methodInfo.GetCustomAttribute<ActionNameAttribute>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(actionNameAttribute);
        }
/*
        [Test]
        public void DeleteConfirmed_GivenValidId_ShouldCallGetById()
        {
            //---------------Set up test pack-------------------
            var id = Guid.NewGuid();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)

                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.DeleteConfirmed(id);
            //---------------Test Result -----------------------
            itemsRepository.Received().GetById(id);
        }

        [Test]
        public void DeleteConfirmed_GivenItemIsReturnedFromRepo_ShouldCallDeleteItem()
        {
            //---------------Set up test pack-------------------
            var item = new ItemBuilder().WithRandomProps().Build();
            var itemsRepository = Substitute.For<IItemsRepository>();
            itemsRepository.GetById(item.ItemId).Returns(item);
            var itemsController = CreateItemsControllerBuilder()
                .WithItemsRepository(itemsRepository)

                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.DeleteConfirmed(item.ItemId);
            //---------------Test Result -----------------------
            itemsRepository.Received().DeleteItem(item);
        }*/



        private static LendingController CreateLendingController(ILendingRepository lendingRepository = null, IMappingEngine mappingEngine = null, IPersonRepository personRepository = null, IItemsRepository itemsRepository = null)
        {
            if (lendingRepository == null)
                lendingRepository = Substitute.For<ILendingRepository>();
            if (mappingEngine == null)
            {
                mappingEngine = Substitute.For<IMappingEngine>();
            }

            if (personRepository == null) personRepository = Substitute.For<IPersonRepository>();
            if (itemsRepository == null) itemsRepository = Substitute.For<IItemsRepository>();
            return new LendingController(lendingRepository, mappingEngine, personRepository, itemsRepository);
        }
    }
}