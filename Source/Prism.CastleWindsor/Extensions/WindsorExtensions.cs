using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Prism.CastleWindsor.Extensions
{
    public static class WindsorExtensions
    {
        /// <summary>
        ///     This is intended to support the resolving of types which are not pre-registered in the container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ResolveType(this IWindsorContainer container, Type type)
        {
            if (type.IsClass && !container.Kernel.HasComponent(type))
                container.Register(Component.For(type).ImplementedBy(type).Named(type.FullName).LifeStyle
                    .Is(LifestyleType.Transient));
            return container.Resolve(type);
        }


        /// <summary>
        ///     This is intended to support the resolving of types which are not pre-registered in the container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ResolveType(this IWindsorContainer container, string name, Type type)
        {
            if (type.IsClass && !container.Kernel.HasComponent(type))
                container.Register(Component.For(type).ImplementedBy(type).Named(name).LifeStyle
                    .Is(LifestyleType.Transient));
            return container.Resolve(type);
        }


        public static T ResolveType<T>(this IWindsorContainer container)
        {
            return (T)ResolveType(container, typeof(T));
        }
    }
}
