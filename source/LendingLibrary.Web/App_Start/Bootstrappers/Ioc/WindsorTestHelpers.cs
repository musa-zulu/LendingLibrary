using System;
using System.Linq;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace LendingLibrary.Web.Bootstrappers.Ioc
{
    public class WindsorTestHelpers
    {
        public IWindsorContainer CreateContainerWith(params IWindsorInstaller[] installer)
        {
            return new WindsorContainer()
                .Install(installer);
        }

        public ComponentModel[] GetComponentModelsFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel)
                .ToArray();
        }

        public Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetComponentModelsFor(type, container)
                .Select(model => model.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        public Type[] GetPublicInterfacesFromApplicationAssembly(Type type, Predicate<Type> @where)
        {
            return type.Assembly.GetExportedTypes()
                .Where(t => t.IsInterface)
                .Where(@where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        public Type[] GetPublicClassesFromApplicationAssembly(Type type, Predicate<Type> @where)
        {
            return type.Assembly.GetExportedTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsAbstract == false)
                .Where(@where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        public IHandler[] GetAllHandlers(IWindsorContainer container)
        {
            return GetHandlersFor(typeof(object), container);
        }

        public IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
        {
            return container.Kernel.GetAssignableHandlers(type);
        }

        public IHandler[] GetHandlersFor<T>(IWindsorContainer container)
        {
            Type type = typeof(T);
            return container.Kernel.GetAssignableHandlers(type);
        }

    }
}