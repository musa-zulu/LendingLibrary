using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
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
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestPersonController
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
            Assert.DoesNotThrow(() => new PersonController(Substitute.For<IPersonRepository>(), Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenIPersonRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new PersonController(null, Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("personRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMappingEngineIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(
                    () => new PersonController(Substitute.For<IPersonRepository>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("mappingEngine", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var personController = CreatePersonController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Index();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Index_shouldCallGetAllFromPersonRepo()
        {
            //---------------Set up test pack-------------------
            var person = new List<Person>();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetAllPeople().Returns(person);
            var personController = CreatePersonController()
                                   .WithPersonRepository(personRepository)
                                   .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Index();
            //---------------Test Result -----------------------
            personRepository.Received(1).GetAllPeople();
        }

        [Test]
        public void Index_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var people = new List<Person> { person };

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetAllPeople().Returns(people);
            var mappingEngine = Substitute.For<IMappingEngine>();

            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Index();
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<List<Person>, List<PersonViewModel>>(people);
        }

        [Test]
        public void Index_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var people = new List<Person> { person };

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetAllPeople().Returns(people);
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var personController = CreatePersonController()
                                    .WithPersonRepository(personRepository)
                                    .WithMappingEngine(mappingEngine)
                                    .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = (ViewResult)personController.Index();
            //---------------Test Result -----------------------
            var model = result.Model;
            Assert.IsInstanceOf<List<PersonViewModel>>(model);
        }

        [Test]
        public void Create_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var personController = CreatePersonController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Create();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_POST_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(PersonController).GetMethod("Create", new[] { typeof(PersonViewModel) });
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
            var methodInfo = typeof(PersonController).GetMethod("Create", new[] { typeof(PersonViewModel) });
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
            var personViewModel = PersonViewModelBuilder.BuildRandom();
            var personController = CreatePersonController()
                                    .WithMappingEngine(mappingEngine)
                                    .Build();

            //---------------Assert Precondition----------------
            Assert.IsTrue(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Create(personViewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<PersonViewModel, Person>(personViewModel);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallSaveOnPersonRepo()
        {
            //---------------Set up test pack-------------------
            var personViewModel = PersonViewModelBuilder.BuildRandom();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
                                    .WithMappingEngine(mappingEngine)
                                    .WithPersonRepository(personRepository)
                                    .Build();

            //---------------Assert Precondition----------------
            Assert.IsTrue(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Create(personViewModel);
            //---------------Test Result -----------------------
            personRepository.Received(1).Save(Arg.Any<Person>());
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldRedirectToIndexPage()
        {
            //---------------Set up test pack-------------------
            var personViewModel = PersonViewModelBuilder.BuildRandom();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
                                    .WithMappingEngine(mappingEngine)
                                    .WithPersonRepository(personRepository)
                                    .Build();

            //---------------Assert Precondition----------------
            Assert.IsTrue(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Create(personViewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [Test]
        public void Create_POST_GivenModelStateIsInvalid_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var personViewModel = PersonViewModelBuilder.BuildRandom();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
                                    .WithMappingEngine(mappingEngine)
                                    .WithPersonRepository(personRepository)
                                    .Build();
            personController.ModelState.AddModelError("key", "some error");

            //---------------Assert Precondition----------------
            Assert.IsFalse(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = (ViewResult)personController.Create(personViewModel);
            //---------------Test Result -----------------------
            var model = result.Model;
            Assert.IsInstanceOf<PersonViewModel>(model);
        }

        [Test]
        public void Edit_GivenIdIsNull_ShouldReturnReturnBadRequest()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
                                    .WithPersonRepository(personRepository)
                                    .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Edit(Guid.Empty) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void Edit_GivenValidId_ShouldCallGetByIdFromRepo()
        {
            //---------------Set up test pack-------------------
            Guid? id = Guid.NewGuid();
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Edit(id);
            //---------------Test Result -----------------------
            personRepository.Received().GetById(id);
        }

        [Test]
        public void Edit_GivenPersonViewModelIsNull_ShouldReturnHttpNotFoundStatus()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
               .WithPersonRepository(personRepository)
               .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Edit((Guid?)null) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void Edit_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var person = new PersonBuilder().WithRandomProps().Build();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(person.PersonId).Returns(person);

            var personController = CreatePersonController()
               .WithPersonRepository(personRepository)
               .WithMappingEngine(mappingEngine)
               .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Edit(person.PersonId);
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<Person, PersonViewModel>(person);
        }

        [Test]
        public void Edit_ShouldReturnViewWithPersonViewModel()
        {
            //---------------Set up test pack-------------------
            var person = new PersonViewModelBuilder().WithRandomProps().Build();
            var personRepository = Substitute.For<IPersonRepository>();
            var mappingEngine = _container.Resolve<IMappingEngine>();
            personRepository.GetById(person.Id).Returns(PersonBuilder.BuildDefault());
            var personController = CreatePersonController()
               .WithPersonRepository(personRepository)
               .WithMappingEngine(mappingEngine)
               .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Edit(person.Id);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PersonViewModel>(person);
        }

        [Test]
        public void Edit_POST_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(PersonController).GetMethod("Edit", new[] { typeof(PersonViewModel) });
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
            var methodInfo = typeof(PersonController).GetMethod("Edit", new[] { typeof(PersonViewModel) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var antiForgeryAttribute = methodInfo.GetCustomAttribute<ValidateAntiForgeryTokenAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(antiForgeryAttribute);
        }

        [Test]
        public void Edit_POST_ShouldReturnViewWithPersonViewModel()
        {
            //---------------Set up test pack-------------------
            var personViewModel = new PersonViewModel();
            var personController = CreatePersonController().Build();
            personController.ModelState.AddModelError("key", "some error");
            //---------------Assert Precondition----------------
            Assert.IsFalse(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Edit(personViewModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallGetByIdFromOrderRepo()
        {
            //---------------Set up test pack-------------------
            var personViewModel = new PersonViewModelBuilder().WithRandomProps().Build();
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .Build();

            //---------------Assert Precondition----------------
            Assert.IsTrue(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Edit(personViewModel);
            //---------------Test Result -----------------------
            personRepository.Received().GetById(personViewModel.Id);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var person = new PersonBuilder().WithRandomProps().Build();
            var personViewModel = new PersonViewModelBuilder().WithRandomProps().Build();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(person.PersonId).Returns(person);
            var mappingEngine = Substitute.For<IMappingEngine>();
            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();

            //---------------Assert Precondition----------------
            Assert.IsTrue(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Edit(personViewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<PersonViewModel, Person>(personViewModel);
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldCallUpdateFromOrderRepo()
        {
            //---------------Set up test pack-------------------
            var existingPerson = PersonBuilder.BuildRandom();
            var personViewModel = new PersonViewModelBuilder().WithRandomProps().Build();

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(personViewModel.Id).Returns(existingPerson);
            var mappingEngine = _container.Resolve<IMappingEngine>();

            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();

            //---------------Assert Precondition----------------
            Assert.IsTrue(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Edit(personViewModel);
            //---------------Test Result -----------------------
            personRepository.Received().Update(existingPerson, Arg.Any<Person>());
        }

        [Test]
        public void Edit_POST_GivenModelStateIsValid_ShouldRedirectToIndex()
        {
            //---------------Set up test pack-------------------
            var existingPerson = PersonBuilder.BuildRandom();
            var personViewModel = new PersonViewModelBuilder().WithRandomProps().Build();

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(personViewModel.Id).Returns(existingPerson);
            var mappingEngine = _container.Resolve<IMappingEngine>();

            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();

            //---------------Assert Precondition----------------
            Assert.IsTrue(personController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = personController.Edit(personViewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [Test]
        public void Delete_GivenInvalidId_ShouldReturnHttpStatusCodeBadRequest()
        {
            //---------------Set up test pack-------------------
            var personController = CreatePersonController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Delete((Guid?)null) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void Delete_GivenValidId_ShouldCallGetByIdFromOrderId()
        {
            //---------------Set up test pack-------------------
            var personId = Guid.NewGuid();
            var personRepository = Substitute.For<IPersonRepository>();
            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Delete(personId) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            personRepository.Received().GetById(personId);
        }

        [Test]
        public void Delete_GivenValidPerson_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var personId = person.PersonId;
            var mappingEngine = Substitute.For<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(personId).Returns(person);
            var personController = CreatePersonController()
                .WithPersonRepository(personRepository)
                .WithMappingEngine(mappingEngine)
                .Build();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Delete(personId) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<Person, PersonViewModel>(person);
        }

        [Test]
        public void Delete_GivenPersonViewModelIsNull_ShouldReturnHttpNotFoundStatus()
        {
            //---------------Set up test pack-------------------
            var personId = Guid.NewGuid();
            var personController = CreatePersonController().Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Delete(personId) as HttpStatusCodeResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void Delete_ShouldReturnViewWithPersonViewModel()
        {
            //---------------Set up test pack-------------------.
            var person = PersonBuilder.BuildRandom();
            var personId = person.PersonId;
            var mappingEngine = _container.Resolve<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(personId).Returns(person);
            var personController = CreatePersonController()
                .WithMappingEngine(mappingEngine)
                .WithPersonRepository(personRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = personController.Delete(personId) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model;
            Assert.IsInstanceOf<PersonViewModel>(model);
        }

        [Test]
        public void DeleteConfirmed_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(PersonController).GetMethod("DeleteConfirmed", new[] { typeof(Guid) });
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
            var methodInfo = typeof(PersonController).GetMethod("DeleteConfirmed", new[] { typeof(Guid) });
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var actionNameAttribute = methodInfo.GetCustomAttribute<ActionNameAttribute>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(actionNameAttribute);
        }

        [Test]
        public void DeleteConfirmed_GivenValidId_ShouldCallGetById()
        {
            //---------------Set up test pack-------------------
            var id = Guid.NewGuid();
            var personRepository = Substitute.For<IPersonRepository>();
            var controller = CreatePersonController()
                .WithPersonRepository(personRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.DeleteConfirmed(id);
            //---------------Test Result -----------------------
            personRepository.Received().GetById(id);
        }
        
        [Test]
        public void DeleteConfirmed_GivenPersonIsReturnedFromRepo_ShouldCallDeletePerson()
        {
            //---------------Set up test pack-------------------
            var person = new PersonBuilder().WithRandomProps().Build();
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(person.PersonId).Returns(person);
            var controller = CreatePersonController()
                .WithPersonRepository(personRepository)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = controller.DeleteConfirmed(person.PersonId);
            //---------------Test Result -----------------------
            personRepository.Received().DeletePerson(person);
        }

        private static PersonControllerBuilder CreatePersonController()
        {
            return new PersonControllerBuilder();
        }
    }
}