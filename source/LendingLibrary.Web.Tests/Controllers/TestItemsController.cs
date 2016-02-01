using System;
using System.Reflection;
using System.Web.Mvc;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Tests.Common.Builders.Domain;
using LendingLibrary.Tests.Common.Builders.ViewModels;
using LendingLibrary.Web.Controllers;
using LendingLibrary.Web.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    //TODO: substitute fake ItemsRepository, add auto mapper
    [TestFixture]
    public class TestItemsController
    {
        [Test]
        public void Contruct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new ItemsController(Substitute.For<IItemsRepository>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenIItemsRepositoryIsNull_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => new ItemsController(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("itemsRepository", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var itemsController = CreateItemsController(itemsRepository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = (ViewResult)itemsController.Index();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }
        
        [Test]
        public void Create_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var itemsController = CreateItemsController(itemsRepository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = (ViewResult)itemsController.Create();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Ignore]
        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallSaveFromItemsRepo()
        {
            //---------------Set up test pack-------------------
            var itemsRepository = Substitute.For<IItemsRepository>();
            var itemViewModel = ItemsViewModelBuilder.BuildRandom();
            var item = ItemBuilder.BuildRandom();
            var itemsController = CreateItemsController(itemsRepository);

            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = itemsController.Create(itemViewModel);
            //---------------Test Result -----------------------
            itemsRepository.Received(1).Save(item);
        }

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldRedirectToItemsIndexPage()
        {
            //---------------Set up test pack-------------------
            var itemsViewModel = new ItemViewModel();
            var itemsController = CreateItemsController();

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
            var itemsController = CreateItemsController();

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
            var methodInfo = typeof (ItemsController).GetMethod("Create", new [] {typeof (ItemViewModel)});
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }
        
        private static ItemsController CreateItemsController(IItemsRepository itemsRepository = null)
        {
            if (itemsRepository == null)
            {
                itemsRepository = Substitute.For<IItemsRepository>();
            }
            return new ItemsController(itemsRepository);
        }
    }
}