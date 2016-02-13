using System.Collections.Generic;
using System.Web.Mvc;
using LendingLibrary.Web.Controllers;
using LendingLibrary.Web.ViewModels;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestHomeController
    {
        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var homeController = CreateHomeController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = homeController.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Index_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var homeController = CreateHomeController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = (ViewResult)homeController.Index();
            //---------------Test Result -----------------------
            var model = result.Model;
            Assert.IsInstanceOf<List<ItemViewModel>>(model);
        }
       

        private static HomeController CreateHomeController()
        {
            return new HomeController();
        }
    }
}