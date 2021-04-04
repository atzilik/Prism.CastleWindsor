using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Prism.CastleWindsor.Ioc;
using Prism.CastleWindsor.Wpf.DialogService;
using Prism.Ioc;
using Prism.Services.Dialogs;

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
            containerRegistry.GetContainer().Register(Component.For<IDialogService>()
                .ImplementedBy(typeof(WindsorDialogService)).LifestyleSingleton().IsDefault());
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
    }
}
