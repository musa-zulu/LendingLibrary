﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.DB.Repository;

namespace LendingLibrary.Web.Bootstrappers.Installers
{
    public class RepositoryInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IItemsRepository>()
                .ImplementedBy<ItemsRepository>()
                .LifestylePerWebRequest());
            container.Register(Component.For<IPersonRepository>()
              .ImplementedBy<PersonRepository>()
              .LifestylePerWebRequest());
            container.Register(Component.For<ILendingRepository>()
             .ImplementedBy<LendingRepository>()
             .LifestylePerWebRequest());
        }
    }
}