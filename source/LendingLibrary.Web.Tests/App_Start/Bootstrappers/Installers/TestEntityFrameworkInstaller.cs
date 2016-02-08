using System.Linq;
using Castle.Core;
using Castle.Core.Internal;
using Castle.Windsor;
using LendingLibrary.DB;
using LendingLibrary.Web.Bootstrappers.Installers;
using LendingLibrary.Web.Bootstrappers.Ioc;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Bootstrappers.Installers
{
    [TestFixture]
    public class TestEntityFrameworkInstaller
    {
        private IWindsorContainer _container;
        private readonly WindsorTestHelpers _windsorTestHelpers = new WindsorTestHelpers();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _container = _windsorTestHelpers.CreateContainerWith(new EntityFrameworkInstaller());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            if (_container != null) _container.Dispose();
        }

        [Test]
        public void Install_Given_ILendingLibraryDbContextt_ShouldHaveImpelemtationForAll()
        {
            //---------------Set up test pack-------------------
            var allHandlers = _windsorTestHelpers.GetAllHandlers(_container);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var controllerHandlers = _windsorTestHelpers.GetHandlersFor<ILendingLibraryDbContext>(_container);
            //---------------Test Result -----------------------
            Assert.That(allHandlers, Is.Not.Empty);
            Assert.That(allHandlers, Is.EqualTo(controllerHandlers));
        }

        [Test]
        public void Install_ShouldRegisterAllControllers()
        {
            //---------------Set up test pack-------------------
            // Is<TType> is an helper, extension method from Windsor in the Castle.Core.Internal namespace
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            var allControllers = _windsorTestHelpers.GetPublicClassesFromApplicationAssembly(typeof(LendingLibraryDbContext), c => c.Is<ILendingLibraryDbContext>());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var registeredControllers = _windsorTestHelpers.GetImplementationTypesFor(typeof(ILendingLibraryDbContext), _container);
            //---------------Test Result -----------------------
            Assert.That(allControllers, Is.EqualTo(registeredControllers));
        }

        [Test]
        public void Install_ShouldRegisterAllEntityFrameworkAsPerWebRequest()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var nonTransientControllers = _windsorTestHelpers.GetHandlersFor(typeof(ILendingLibraryDbContext), _container)
                .Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.PerWebRequest)
                .ToArray();
            //---------------Test Result -----------------------

            Assert.That(nonTransientControllers, Is.Empty);
        }
    }
}