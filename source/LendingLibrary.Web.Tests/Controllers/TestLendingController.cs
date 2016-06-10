using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using Castle.Windsor;
using LendingLibrary.Core;
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
            var ex = Assert.Throws<ArgumentNullException>(() => new LendingController(Substitute.For<ILendingRepository>(), null, Substitute.For<IPersonRepository>(), Substitute.For<IItemsRepository>()));
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
            var ex = Assert.Throws<ArgumentNullException>(() => new LendingController(Substitute.For<ILendingRepository>(), Substitute.For<IMappingEngine>(), null, Substitute.For<IItemsRepository>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("personRepository", ex.ParamName);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProvider_ShouldSetDateTimeProviderOnFirstCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController().Build();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            controller.DateTimeProvider = dateTimeProvider;
            //---------------Test Result -----------------------
            Assert.AreSame(dateTimeProvider, controller.DateTimeProvider);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProviderIsSet_ShouldThrowOnCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController().Build();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var dateTimeProvider1 = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<InvalidOperationException>(() => controller.DateTimeProvider = dateTimeProvider1);
            //---------------Test Result -----------------------
            Assert.AreEqual("DateTimeProvider is already set", ex.Message);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Index();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
        }

        [Test]
        public void Index_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController().Build();
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
            var controller = CreateLendingController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Create();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
        }

        [Test]
        public void Create_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = (ViewResult)controller.Create();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            var model = results.Model;
            Assert.IsInstanceOf<LendingViewModel>(model);
        }

        [Test]
        public void Create_ShouldCallGetAllPeopleFromPersonRepo()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var controller = CreateLendingController(null, null, personRepository, null);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Create();
            //---------------Test Result -----------------------
            personRepository.Received().GetAllPeople();
        }

        [Test]
        public void Create_ShouldCallGetAllItemsFromItemsRepo()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var controller = CreateLendingController(null, null, null, itemsRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Create();
            //---------------Test Result -----------------------
            itemsRepository.Received().GetAllItems();
        }

        [Test]
        public void Create_GivenNoPeopleExist_ShouldNotSetPeopleSelectList()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.PeopleSelectList);
            Assert.AreEqual(0, viewModel.PeopleSelectList.Count());
        }

        [Test]
        public void Create_GivenOnePersonExistOnPersornRepo_ShouldSetPeopleSelectList()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var person = new PersonBuilder().WithRandomProps().Build();
            personRepository.GetAllPeople().Returns(new List<Person> { person });
            var controller = CreateLendingController(null, null, personRepository, null);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.PeopleSelectList);
            Assert.AreEqual(person.PersonId.ToString(), viewModel.PeopleSelectList.FirstOrDefault().Value);
            Assert.AreEqual(person.FirstName, viewModel.PeopleSelectList.FirstOrDefault().Text);
            Assert.AreEqual(1, viewModel.PeopleSelectList.Count());
        }

        [Test]
        public void Create_GivenTwoPeopleExistOnPersornRepo_ShouldSetPeopleSelectList()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var person1 = new PersonBuilder().Build();
            var person2 = new PersonBuilder().Build();
            personRepository.GetAllPeople().Returns(new List<Person> { person1, person2 });
            var controller = CreateLendingController(null, null, personRepository, null);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.PeopleSelectList);
            Assert.AreEqual(2, viewModel.PeopleSelectList.Count());
        }

        [Test]
        public void Create_GivenManyPeopleExistOnPersornRepo_ShouldSetPeopleSelectList()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var person1 = new PersonBuilder().Build();
            var person2 = new PersonBuilder().Build();
            var person3 = new PersonBuilder().Build();
            personRepository.GetAllPeople().Returns(new List<Person> { person1, person2, person3 });
            var controller = CreateLendingController(null, null, personRepository, null);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.PeopleSelectList);
            Assert.AreEqual(3, viewModel.PeopleSelectList.Count());
        }

        [Test]
        public void Create_GivenNoItemExist_ShouldNotSetItemsSelectList()
        {
            //---------------Set up test pack-------------------
            var controller = CreateLendingController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.ItemsSelectList);
            Assert.AreEqual(0, viewModel.ItemsSelectList.Count());
        }

        [Test]
        public void Create_GivenOneItemExist_ShouldSetItemsSelectList()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var item = new ItemBuilder().WithRandomProps().Build();
            itemsRepository.GetAllItems().Returns(new List<Item> { item });
            var controller = CreateLendingController(null, null, null, itemsRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.ItemsSelectList);
            Assert.AreEqual(item.ItemId.ToString(), viewModel.ItemsSelectList.FirstOrDefault().Value);
            Assert.AreEqual(item.ItemName, viewModel.ItemsSelectList.FirstOrDefault().Text);
            Assert.AreEqual(1, viewModel.ItemsSelectList.Count());
        }

        [Test]
        public void Create_GivenTwoItemsExist_ShouldSetItemsSelectList()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var item1 = new ItemBuilder().WithRandomProps().Build();
            var item2 = new ItemBuilder().WithRandomProps().Build();
            itemsRepository
                .GetAllItems()
                .Returns(new List<Item> { item1, item2 });
            var controller = CreateLendingController(null, null, null, itemsRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.ItemsSelectList);
            Assert.AreEqual(2, viewModel.ItemsSelectList.Count());
        }

        [Test]
        public void Create_GivenThreeItemsExist_ShouldSetItemsSelectList()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var item1 = new ItemBuilder().WithRandomProps().Build();
            var item2 = new ItemBuilder().WithRandomProps().Build();
            var item3 = new ItemBuilder().WithRandomProps().Build();
            itemsRepository
                .GetAllItems()
                .Returns(new List<Item> { item1, item2, item3 });
            var controller = CreateLendingController(null, null, null, itemsRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.ItemsSelectList);
            Assert.AreEqual(3, viewModel.ItemsSelectList.Count());
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

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = Substitute.For<IMappingEngine>();
            var viewModel = new LendingViewModel();
            var controller = CreateLendingController(null, mappingEngine);
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<LendingViewModel, Lending>(viewModel);
        }

        [Test]
        public void Create_POST_GivenItemIdIsNotNull_ShouldSetItemName()
        {
            //---------------Set up test pack-------------------
            var item = new ItemBuilder().WithRandomProps().Build();
            var person = new PersonBuilder().Build();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var personRepository = Substitute.For<IPersonRepository>();
            var lending = new LendingBuilder().WithRandomProps().Build();
            var viewModel = new LendingViewModelBuilder().WithRandomProps().Build();
            mappingEngine.Map<LendingViewModel, Lending>(viewModel).Returns(lending);
            itemsRepository.GetById(viewModel.ItemId).Returns(item);
            personRepository.GetById(viewModel.PersonId).Returns(person);
            var controller = CreateLendingController(null, mappingEngine, personRepository, itemsRepository);
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.AreEqual(item.ItemName, lending.ItemName);
        }

        [Test]
        public void Create_POST_GivenPersonIdIsNotNull_ShouldSetPersonName()
        {
            //---------------Set up test pack-------------------
            var item = new ItemBuilder().WithRandomProps().Build();
            var person = new PersonBuilder().WithRandomProps().Build();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var personRepository = Substitute.For<IPersonRepository>();
            var lending = new LendingBuilder().WithRandomProps().Build();
            var viewModel = new LendingViewModelBuilder().WithRandomProps().Build();
            mappingEngine.Map<LendingViewModel, Lending>(viewModel).Returns(lending);
            itemsRepository.GetById(viewModel.ItemId).Returns(item);
            personRepository.GetById(viewModel.PersonId).Returns(person);
            var controller = CreateLendingController(null, mappingEngine, personRepository, itemsRepository);
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.AreEqual(lending.PersonName, person.FirstName);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallSaveOnPersonRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = new LendingViewModel();
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
        public void Create_POST_GivenModelStateIsValid_ShouldSaveLendingEntryToDB()
        {
            //---------------Set up test pack-------------------
            var item = new ItemBuilder().WithRandomProps().Build();
            var person = new PersonBuilder().WithRandomProps().Build();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var personRepository = Substitute.For<IPersonRepository>();
            var lendingRepository = Substitute.For<ILendingRepository>();
            var lending = new LendingBuilder().WithRandomProps().Build();
            var viewModel = new LendingViewModelBuilder().WithRandomProps().Build();
            mappingEngine.Map<LendingViewModel, Lending>(viewModel).Returns(lending);
            itemsRepository.GetById(viewModel.ItemId).Returns(item);
            personRepository.GetById(viewModel.PersonId).Returns(person);
            var controller = CreateLendingController(lendingRepository, mappingEngine, personRepository, itemsRepository);

            Lending savedLending = null;
            lendingRepository.When(x => x.Save(Arg.Any<Lending>())).Do(info => savedLending = (Lending)info[0]);
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.AreEqual(lending.LedingId, savedLending.LedingId);
            Assert.AreEqual(lending.ItemId, savedLending.ItemId);
            Assert.AreEqual(lending.PersonId, savedLending.PersonId);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldRedirectToIndexPage()
        {
            //---------------Set up test pack-------------------
            var viewModel = new LendingViewModel();
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
        }

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
            var itemsController = CreateLendingController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = itemsController.Edit(Guid.Empty);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        //[Test]
        //public void Edit_GivenIdIsNull_ShouldReturnBadRequest()
        //{
        //    //---------------Set up test pack-------------------
        //    var controller = CreateLendingController().Build();

        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(Guid.Empty) as HttpStatusCodeResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        //}

        //[Test]
        //public void Edit_GivenItemId_ShouldCallGetByIdFromTheLendingsRepo()
        //{
        //    //---------------Set up test pack-------------------
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    var controller = CreateLendingController(lendingRepository);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    lendingRepository.Received(1).GetById(id);
        //}

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

        //[Test]
        //public void Edit_GivenViewModelIsNull_ShouldReturnHttpNotFoundStatus()
        //{
        //    //---------------Set up test pack-------------------
        //    var controller = CreateLendingController().Build();
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit((Guid?)null) as HttpStatusCodeResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        //}

        [Test]
        public void Edit_ShouldCallGetAllPeopleFromPersonRepo()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var lending = new LendingBuilder().WithRandomGeneratedId().Build();
            var id = lending.LedingId;
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var lendingRepository = Substitute.For<ILendingRepository>();
            lendingRepository.GetById(id).Returns(lending);
            var controller = CreateLendingController(lendingRepository, mappingEngine, personRepository);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.Edit(id);
            //---------------Test Result -----------------------
            personRepository.Received().GetAllPeople();
        }

        //[Test]
        //public void Edit_GivenNoPersonIsReturnedFromDB_ShouldNotSetPersonSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    lendingRepository.GetById(id).Returns(lending);
        //    var controller = CreateLendingController(lendingRepository, mappingEngine);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(0, model.PeopleSelectList.Count());
        //}

        //[Test]
        //public void Edit_GivenOnePersonIsReturnedFromDB_ShouldSetPersonSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    var personRepository = Substitute.For<IPersonRepository>();
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    lendingRepository.GetById(id).Returns(lending);
        //    var person = new PersonBuilder().WithRandomProps().Build();
        //    personRepository.GetAllPeople().Returns(new List<Person> { person });
        //    var controller = CreateLendingController(lendingRepository, mappingEngine, personRepository);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(1, model.PeopleSelectList.Count());
        //    Assert.AreEqual(person.PersonId.ToString(), model.PeopleSelectList.FirstOrDefault().Value);
        //    Assert.AreEqual(person.FirstName, model.PeopleSelectList.FirstOrDefault().Text);
        //}

        //[Test]
        //public void Edit_GivenTwoPeopleAreReturnedFromDB_ShouldSetPersonSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    var personRepository = Substitute.For<IPersonRepository>();
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    lendingRepository.GetById(id).Returns(lending);
        //    var person1 = new PersonBuilder().WithRandomProps().Build();
        //    var person2 = new PersonBuilder().WithRandomProps().Build();
        //    personRepository.GetAllPeople().Returns(new List<Person> { person1, person2 });
        //    var controller = CreateLendingController(lendingRepository, mappingEngine, personRepository);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(2, model.PeopleSelectList.Count());
        //}

        //[Test]
        //public void Edit_GivenManyPeopleAreReturnedFromDB_ShouldSetPersonSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    var personRepository = Substitute.For<IPersonRepository>();
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    lendingRepository.GetById(id).Returns(lending);
        //    var person1 = new PersonBuilder().WithRandomProps().Build();
        //    var person2 = new PersonBuilder().WithRandomProps().Build();
        //    var person3 = new PersonBuilder().WithRandomProps().Build();
        //    personRepository.GetAllPeople().Returns(new List<Person> { person1, person2, person3 });

        //    var controller = CreateLendingController()
        //                    .WithLendingRepository(lendingRepository)
        //                    .WithMappingEngine(mappingEngine)
        //                    .WithPersonRepository(personRepository)
        //                    .Build();
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(3, model.PeopleSelectList.Count());
        //}

        //[Test]
        //public void Edit_GivenNoItemIsReturnedFromDB_ShouldNotSetItemSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    lendingRepository.GetById(id).Returns(lending);
        //    var controller = CreateLendingController(lendingRepository, mappingEngine);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(0, model.ItemsSelectList.Count());
        //}

        //[Test]
        //public void Edit_GivenOneItemIsReturnedFromDB_ShouldSetItemSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    var itemRepository = Substitute.For<IItemsRepository>();
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    lendingRepository.GetById(id).Returns(lending);
        //    var item = new ItemBuilder().WithRandomProps().Build();
        //    itemRepository.GetAllItems().Returns(new List<Item> { item });
        //    var controller = CreateLendingController(lendingRepository, mappingEngine, null, itemRepository);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(1, model.ItemsSelectList.Count());
        //    Assert.AreEqual(item.ItemId.ToString(), model.ItemsSelectList.FirstOrDefault().Value);
        //    Assert.AreEqual(item.ItemName, model.ItemsSelectList.FirstOrDefault().Text);
        //}

        //[Test]
        //public void Edit_GivenTwoItemsAreReturnedFromDB_ShouldSetItemSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    var itemRepository = Substitute.For<IItemsRepository>();
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    lendingRepository.GetById(id).Returns(lending);
        //    var item1 = new ItemBuilder().WithRandomProps().Build();
        //    var item2 = new ItemBuilder().WithRandomProps().Build();
        //    itemRepository.GetAllItems().Returns(new List<Item> { item1, item2 });
        //    var controller = CreateLendingController(lendingRepository, mappingEngine, null, itemRepository);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(2, model.ItemsSelectList.Count());
        //}

        //[Test]
        //public void Edit_GivenManyItemsAreReturnedFromDB_ShouldSetItemsSelectList()
        //{
        //    //---------------Set up test pack-------------------
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    var itemRepository = Substitute.For<IItemsRepository>();
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    lendingRepository.GetById(id).Returns(lending);
        //    var item1 = new ItemBuilder().WithRandomProps().Build();
        //    var item2 = new ItemBuilder().WithRandomProps().Build();
        //    var item3 = new ItemBuilder().WithRandomProps().Build();
        //    itemRepository.GetAllItems().Returns(new List<Item> { item1, item2, item3 });
        //    var controller = CreateLendingController(lendingRepository, mappingEngine, null, itemRepository);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model as LendingViewModel;
        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(3, model.ItemsSelectList.Count());
        //}

        //[Test]
        //public void Edit_ShouldReturnViewWithItemViewModel()
        //{
        //    //---------------Set up test pack-------------------
        //    var lending = new LendingBuilder().WithRandomGeneratedId().Build();
        //    var id = lending.LedingId;
        //    var mappingEngine = _container.Resolve<IMappingEngine>();
        //    var lendingRepository = Substitute.For<ILendingRepository>();
        //    lendingRepository.GetById(id).Returns(lending);
        //    var controller = CreateLendingController(lendingRepository, mappingEngine);
        //    //---------------Assert Precondition----------------

        //    //---------------Execute Test ----------------------
        //    var result = controller.Edit(id) as ViewResult;
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(result);
        //    var model = result.Model;
        //    Assert.IsNotNull(model);
        //    Assert.IsInstanceOf<LendingViewModel>(model);
        //}

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

        //[Test]
        //public void Edit_POST_ShouldHaveValidateAntiForgeryTokenAttribute()
        //{
        //    //---------------Set up test pack-------------------
        //    var methodInfo = typeof(LendingController).GetMethod("Edit", new[] { typeof(LendingViewModel) });
        //    //---------------Assert Precondition----------------
        //    Assert.IsNotNull(methodInfo);
        //    //---------------Execute Test ----------------------
        //    var antiForgeryAttribute = methodInfo.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>();
        //    //---------------Test Result -----------------------
        //    Assert.NotNull(antiForgeryAttribute);
        //}

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallGetByIdFromLendingsRepo()
        {
            //---------------Set up test pack-------------------
            var lendingsViewModel = new LendingViewModel();
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
            var viewModel = new LendingViewModel();
            var mappingEngine = Substitute.For<IMappingEngine>();

            var controller = CreateLendingController(null, mappingEngine);
            //---------------Assert Precondition----------------
            Assert.IsTrue(controller.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<LendingViewModel, Lending>(viewModel);
        }

        [Test]
        public void Edit_POST_GivenItemIdIsEmpty_ShouldNotCallGetByIdFromItemsRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = new LendingViewModel();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var controller = CreateLendingController()
                .WithItemsRepository(itemsRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            itemsRepository.DidNotReceive().GetById(viewModel.ItemId);
        }

        [Test]
        public void Edit_POST_GivenItemIdIsNotNull_ShouldCallGetByIdFromItemsRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingViewModelBuilder.BuildRandom();
            var item = ItemBuilder.BuildRandom();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(viewModel.PersonId).Returns(PersonBuilder.BuildRandom());
            itemsRepository.GetById(viewModel.ItemId).Returns(item);
            var controller = CreateLendingController()
                .WithItemsRepository(itemsRepository)
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            itemsRepository.Received().GetById(viewModel.ItemId);
        }

        [Test]
        public void Edit_POST_GivenItemIdIsNotNull_ShouldSetItemName()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingViewModelBuilder.BuildRandom();
            var item = ItemBuilder.BuildRandom();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var lending = LendingBuilder.BuildRandom();
            mappingEngine.Map<LendingViewModel, Lending>(viewModel).Returns(lending);
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(viewModel.PersonId).Returns(PersonBuilder.BuildRandom());
            itemsRepository.GetById(viewModel.ItemId).Returns(item);
            var controller = CreateLendingController()
                .WithItemsRepository(itemsRepository)
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            Assert.AreEqual(lending.ItemName, item.ItemName);
        }

        [Test]
        public void Edit_POST_GivenPersonIdIsEmpty_ShouldNotCallGetByIdFromPersonsRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = new LendingViewModel();
            var personRepository = Substitute.For<IPersonRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var controller = CreateLendingController()
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            personRepository.DidNotReceive().GetById(viewModel.ItemId);
        }

        [Test]
        public void Edit_POST_GivenPersonIdIsNotNull_ShouldCallGetByIdFromPersonsRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingViewModelBuilder.BuildRandom();
            var item = ItemBuilder.BuildRandom();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(viewModel.PersonId).Returns(PersonBuilder.BuildRandom());
            itemsRepository.GetById(viewModel.ItemId).Returns(item);
            var controller = CreateLendingController()
                .WithItemsRepository(itemsRepository)
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            personRepository.Received().GetById(viewModel.PersonId);
        }

        [Test]
        public void Edit_POST_GivenPersonIdIsNotNull_ShouldSetPersonName()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingViewModelBuilder.BuildRandom();
            var item = ItemBuilder.BuildRandom();
            var itemsRepository = Substitute.For<IItemsRepository>();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var lending = LendingBuilder.BuildRandom();
            mappingEngine.Map<LendingViewModel, Lending>(viewModel).Returns(lending);
            var personRepository = Substitute.For<IPersonRepository>();
            var person = PersonBuilder.BuildRandom();
            personRepository.GetById(viewModel.PersonId).Returns(person);
            itemsRepository.GetById(viewModel.ItemId).Returns(item);
            var controller = CreateLendingController()
                .WithItemsRepository(itemsRepository)
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            Assert.AreEqual(lending.PersonName, person.FirstName);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallUpdateFromLendingsRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = new LendingViewModel();
            var lendingRepository = Substitute.For<ILendingRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var itemsController = CreateLendingController()
                                .WithLendingRepository(lendingRepository)
                                .WithMappingEngine(mappingEngine)
                                .Build();
            //---------------Assert Precondition----------------
            Assert.IsTrue(itemsController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = itemsController.Edit(viewModel);
            //---------------Test Result -----------------------
            lendingRepository.Received().Update(Arg.Any<Lending>(), Arg.Any<Lending>());
        }

         [Test]
         public void Edit_POST_GivenModelStateIsValid_ShouldRedirectToLendingsIndexPage()
         {
             //---------------Set up test pack-------------------
             var viewModel = new LendingViewModel();
             var controller = CreateLendingController().Build();
             //---------------Assert Precondition----------------
             Assert.IsTrue(controller.ModelState.IsValid);
             //---------------Execute Test ----------------------
             var result = controller.Edit(viewModel) as RedirectToRouteResult;
             //---------------Test Result -----------------------
             Assert.IsNotNull(result);
             Assert.AreEqual("Index", result.RouteValues["Action"]);
         }

        [Test]
         public void Edit_POST_GivenModelStateIsInvalid_ShouldReturnViewWithViewLendingViewModel()
         {
             //---------------Set up test pack-------------------
             var viewModel = new LendingViewModel();
             var controller = CreateLendingController().Build();
            controller.ModelState.AddModelError("key", "error message");
             //---------------Assert Precondition----------------
             Assert.IsFalse(controller.ModelState.IsValid);
             //---------------Execute Test ----------------------
             var result = controller.Edit(viewModel) as ViewResult;
             //---------------Test Result -----------------------
             Assert.IsNotNull(result);
             var model = result.Model;
             Assert.IsNotNull(model);
             Assert.IsInstanceOf<LendingViewModel>(model);
         }

       //[Test]
       //public void Delete_GivenItemIdIsNull_ShouldReturnHttpStatusOfBadRequest()
       //{
       //    //---------------Set up test pack-------------------
       //    var itemsRepository = Substitute.For<IItemsRepository>();
       //    var itemsControllerBuilder = CreateLendingController()
       //                                .WithItemsRepository(itemsRepository)
       //                                .Build();
       //    //---------------Assert Precondition----------------
       //    //---------------Execute Test ----------------------
       //    var result = itemsControllerBuilder.Delete((Guid?)null) as HttpStatusCodeResult;
       //    //---------------Test Result -----------------------
       //    Assert.IsNotNull(result);
       //    Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
       //}

       // /*    [Test]
           //public void Delete_GivenAValidId_ShouldCallGetById()
           //{
           //    //---------------Set up test pack-------------------
           //    var itemsRepository = Substitute.For<IItemsRepository>();
           //    var itemsControllerBuilder = CreateItemsControllerBuilder()
           //                                .WithItemsRepository(itemsRepository)
           //                                .Build();
           //    var item = ItemBuilder.BuildRandom();
           //    //---------------Assert Precondition----------------

           //    //---------------Execute Test ----------------------
           //    itemsControllerBuilder.Delete(item.ItemId);
           //    //---------------Test Result -----------------------
           //    itemsRepository.Received(1).GetById(item.ItemId);
           //}

           //[Test]
           //public void Delete_GivenAnItemIsReturnedFromRepo_ShouldcallMappingEngine()
           //{
           //    //---------------Set up test pack-------------------
           //    var mappingEngine = Substitute.For<IMappingEngine>();
           //    var itemsRepository = Substitute.For<IItemsRepository>();
           //    var item = ItemBuilder.BuildRandom();
           //    itemsRepository.GetById(item.ItemId).Returns(item);
           //    var itemsControllerBuilder = CreateItemsControllerBuilder()
           //                                .WithMappingEngine(mappingEngine)
           //                                .WithItemsRepository(itemsRepository)
           //                                .Build();
           //    //---------------Assert Precondition----------------

           //    //---------------Execute Test ----------------------
           //    itemsControllerBuilder.Delete(item.ItemId);
           //    //---------------Test Result -----------------------
           //    mappingEngine.Received(1).Map<Item, ItemViewModel>(item);
           //}

           //[Test]
           //public void Delete_GivenItemsViewModelIsNull_ShouldReturnHttpNotFound()
           //{
           //    //---------------Set up test pack-------------------
           //    var itemsController = CreateItemsControllerBuilder().Build();
           //    //---------------Assert Precondition----------------

           //    //---------------Execute Test ----------------------
           //    var result = itemsController.Delete(Guid.NewGuid()) as HttpStatusCodeResult;
           //    //---------------Test Result -----------------------
           //    Assert.IsNotNull(result);
           //    Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
           //}

           //[Test]
           //public void Delete_ShouldReturnViewWithItemsViewModel()
           //{
           //    //---------------Set up test pack-------------------
           //    var item = new ItemBuilder().WithRandomProps().Build();
           //    var mappingEngine = _container.Resolve<IMappingEngine>();
           //    var itemsRepository = Substitute.For<IItemsRepository>();
           //    itemsRepository.GetById(item.ItemId).Returns(item);
           //    var itemsController = CreateItemsControllerBuilder()
           //        .WithItemsRepository(itemsRepository)
           //        .WithMappingEngine(mappingEngine)
           //        .Build();
           //    //---------------Assert Precondition----------------

           //    //---------------Execute Test ----------------------
           //    var result = itemsController.Delete(item.ItemId) as ViewResult;
           //    //---------------Test Result -----------------------
           //    Assert.IsNotNull(result);
           //    var model = result.Model;
           //    Assert.IsInstanceOf<ItemViewModel>(model);
           //}
    
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

        private static LendingControllerBuilder CreateLendingController()
        {
            return new LendingControllerBuilder();
        }
    }
}