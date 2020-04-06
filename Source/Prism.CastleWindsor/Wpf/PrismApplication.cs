using System;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator;
using Prism.CastleWindsor.Extensions;
using Prism.CastleWindsor.Ioc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace Prism.CastleWindsor
{
    /// <inheritdoc />
    public abstract class PrismApplication : PrismApplicationBase
    {
        /// <inheritdoc />
        protected override IContainerExtension CreateContainerExtension()
        {
            return new CastleWindsorContainerExtension();
        }

        /// <inheritdoc />
        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            this.Container.GetContainer().Register((IRegistration)Component.For<IWindsorContainer>().Instance(Container.GetContainer()));
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, RegionNavigationContentLoader>();
            containerRegistry.RegisterSingleton<IServiceLocator, CastleWindsorServiceLocatorAdapter>();
        }

        /// <inheritdoc />
        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();

            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentResolutionException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentNotFoundException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ComponentRegistrationException));
            ExceptionExtensions.RegisterFrameworkExceptionType(typeof(CircularDependencyException));
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) => Container.GetContainer().ResolveType(type));
        }
    }
}
