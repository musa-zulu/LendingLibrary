using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests
{
    [TestFixture]
    public class TestWindsorControllerFactory
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new WindsorControllerFactory(Substitute.For<IWindsorContainer>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void GetControllerInstance_GivenControllerTypeIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IWindsorContainer>();
            var controllerFactory = CreateControllerFactoryImpl(container);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<HttpException>(() =>
            {
                var requestContext = new RequestContext { HttpContext = Substitute.For<HttpContextBase>() };
                controllerFactory.CallGetControllerInstance(requestContext, null);
            });
            //---------------Test Result -----------------------
            var httpCode = ex.GetHttpCode();
            Assert.That(httpCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            Assert.That(ex.Message, Contains.Substring("The controller for path '' could not be found."));
        }

        [Test]
        public void GetControllerInstance_GivenValidControllerType_ShouldCallResolveAndReturnIController()
        {
            //---------------Set up test pack-------------------
            var controller = Substitute.For<IController>();
            var container = Substitute.For<IWindsorContainer>();
            container.Resolve(controller.GetType()).Returns(controller);
            var controllerFactory = CreateControllerFactoryImpl(container);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var requestContext = new RequestContext { HttpContext = Substitute.For<HttpContextBase>() };
            var controllerInstance = controllerFactory.CallGetControllerInstance(requestContext, controller.GetType());
            //---------------Test Result -----------------------
            Assert.That(controllerInstance, Is.SameAs(controller));
        }

        [Test]
        public void ReleaseController_ShouldCallReleaseOnContainer()
        {
            //---------------Set up test pack-------------------
            var controller = Substitute.For<IController>();
            var container = Substitute.For<IWindsorContainer>();
            var controllerFactory = CreateControllerFactory(container);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            controllerFactory.ReleaseController(controller);
            //---------------Test Result -----------------------
            container.Received(1).Release(controller);
        }

        private static WindsorControllerFactory CreateControllerFactory(IWindsorContainer windsorContainer)
        {
            return new WindsorControllerFactory(windsorContainer);
        }

        private static WindsorControllerFactoryImpl CreateControllerFactoryImpl(IWindsorContainer windsorContainer)
        {
            return new WindsorControllerFactoryImpl(windsorContainer);
        }

        internal class WindsorControllerFactoryImpl : WindsorControllerFactory
        {
            public WindsorControllerFactoryImpl(IWindsorContainer container)
                : base(container)
            {
            }

            public IController CallGetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}
