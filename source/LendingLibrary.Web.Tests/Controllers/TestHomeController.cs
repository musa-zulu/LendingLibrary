using System.Web.Mvc;
using LendingLibrary.Web.Controllers;
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
            var homeController = new HomeController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actual = homeController.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(actual);
        }
    }
}