using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Prism.CastleWindsor.Extensions;
using Prism.Ioc;

namespace Prism.CastleWindsor.Ioc
{
    public class CastleWindsorContainerExtension : IContainerExtension<IWindsorContainer>
    {
        public IWindsorContainer Instance { get; }

        public CastleWindsorContainerExtension() : this(new WindsorContainer())
        {
        }

        public CastleWindsorContainerExtension(IWindsorContainer container)
        {
            Instance = container;
        }



        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.Register(Component.For(type).Instance(instance));
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.Register(Component.For(type).Instance(instance).Named(name));
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleSingleton());
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type @from, Type to, string name)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleSingleton().Named(name));
            return this;
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleTransient());
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).Named(name).LifestyleTransient());
            return this;
        }
        public object Resolve(Type type)
        {
            return Instance.ResolveType(type);
        }

        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            Arguments arguments = new Arguments
            {
                parameters.Select(p => new KeyValuePair<object, object>(p.Type, p.Instance))
            };
            return Instance.Resolve(type, arguments);
        }

        public object Resolve(Type type, string name)
        {
            return Instance.Resolve(name, type);
        }

        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            Arguments arguments = new Arguments
            {
                parameters.Select(p => new KeyValuePair<object, object>(p.Type, p.Instance))
            };
            return Instance.Resolve(name, type, arguments);
        }
        public bool IsRegistered(Type type)
        {
            return Instance.Kernel.HasComponent(type);
        }

        public bool IsRegistered(Type type, string name)
        {
            return Instance.Kernel.HasComponent(type);
        }

        public void FinalizeExtension() { }

    }
}
