using System.Web.Mvc;
using LendingLibrary.Web.Controllers;
using LendingLibrary.Web.ViewModels;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestSecurityController
    {
        [Test]
        public void Login_ShouldReturnViewResult()
        {
            //---------------Set up test pack-------------------
            var securityController = CreateSecurityController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actual = securityController.Login();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<ViewResult>(actual);
        }

        [Test]
        public void Login_ShouldReturnViewWithSecurityViewModel()
        {
            //---------------Set up test pack-------------------
            var securityController = CreateSecurityController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actual = (ViewResult)securityController.Login();
            //---------------Test Result -----------------------
            var model = actual.Model;
            Assert.IsInstanceOf<SecurityViewModel>(model);
        }

        [Test]
        public void Login_POST_GivenInCorrectUserNameAndPassword_ShouldReturnLoginView()
        {
            //---------------Set up test pack-------------------
            var securityController = CreateSecurityController();
            var securityViewModel = new SecurityViewModel
            {
                UserName = "Cmusa",
                Password = "Cmusa"
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actual = (ViewResult)securityController.Login(securityViewModel);
            //---------------Test Result -----------------------
            Assert.AreEqual("Login", actual.ViewName);
        }
        [Ignore("Dummy tests")]
        [Test]
        public void Login_POST_GivenCorrectUserNameAndPassword_ShouldRedirectToHomeIndex()
        {
            //---------------Set up test pack-------------------
            var securityController = CreateSecurityController();
            var securityViewModel = new SecurityViewModel
            {
                UserName = "Musa",
                Password = "Musa"
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var actual = (RedirectToRouteResult)securityController.Login(securityViewModel);
            //---------------Test Result -----------------------
            Assert.AreEqual("Index", actual.RouteValues["action"]);
            Assert.AreEqual("Home", actual.RouteValues["controller"]);
        }

        private static SecurityController CreateSecurityController()
        {
            return new SecurityController();
        }
    }
}