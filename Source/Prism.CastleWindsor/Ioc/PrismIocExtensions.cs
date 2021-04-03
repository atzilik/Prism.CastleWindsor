using System;
using Castle.Windsor;
using Prism.CastleWindsor.Wpf.DialogService;
using Prism.Ioc;

namespace Prism.CastleWindsor.Ioc
{
    public static class PrismIocExtensions
    {
        public static IWindsorContainer GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<IWindsorContainer>) containerProvider).Instance;
        }

        public static IWindsorContainer GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<IWindsorContainer>) containerRegistry).Instance;
        }

        /// <summary>
            /// Registers an object for navigation in Castle Windsor
            /// </summary>
            /// <param name="containerRegistry"></param>
            /// <param name="type">The type of object to register</param>
            /// <param name="name">The unique name to register with the object.</param>
            public static void RegisterForNavigation(this IContainerRegistry containerRegistry, Type type, string name)
            {
                containerRegistry.Register(typeof(INavigation), type, name);
            }
    }
}
