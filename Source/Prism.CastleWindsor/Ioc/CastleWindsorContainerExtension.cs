using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Diagnostics.Helpers;
using Prism.CastleWindsor.Extensions;
using Prism.Ioc;
using Prism.Ioc.Internals;

namespace Prism.CastleWindsor.Ioc
{
#if ContainerExtensions
    internal partial
#else
    public
#endif
    class CastleWindsorContainerExtension : IContainerExtension<IWindsorContainer>, IContainerInfo
    {
        private WindsorScopedProvider _currentScope;

        public IWindsorContainer Instance { get; }

        public CastleWindsorContainerExtension() : this(new WindsorContainer())
        {
        }
#if !ContainerExtensions
        public CastleWindsorContainerExtension(IWindsorContainer container)
        {
            Instance = container;
            string currentContainer = "CurrentContainer";
            Instance.Register(Component.For<CastleWindsorContainerExtension>().Instance(this).Named(currentContainer));
            Instance.Register(Component.For<IContainerExtension>().UsingFactoryMethod(x => x.Resolve<CastleWindsorContainerExtension>(currentContainer)));
            Instance.Register(Component.For<IContainerProvider>().UsingFactoryMethod(x => x.Resolve<CastleWindsorContainerExtension>(currentContainer)));
        }
#endif
        /// <summary>
        /// Gets the current <see cref="IScopedProvider"/>
        /// </summary>
        public IScopedProvider CurrentScope => _currentScope;

        /// <summary>
        /// Registers an instance of a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The service <see cref="Type"/> that is being registered</param>
        /// <param name="instance">The instance of the service or <see cref="Type" /></param>
        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.Register(Component.For(type).Instance(instance));
            return this;
        }

        /// <summary>
        /// Registers an instance of a given <see cref="Type"/> with the specified name or key
        /// </summary>
        /// <param name="type">The service <see cref="Type"/> that is being registered</param>
        /// <param name="instance">The instance of the service or <see cref="Type" /></param>
        /// <param name="name">The name or key to register the service</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.Register(Component.For(type).Instance(instance).Named(name));
            return this;
        }


        /// <summary>
        /// Registers a Singleton with the given service and mapping to the specified implementation <see cref="Type" />.
        /// </summary>
        /// <param name="from">The service <see cref="Type" /></param>
        /// <param name="to">The implementation <see cref="Type" /></param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleSingleton());
            return this;
        }

        /// <summary>
        /// Registers a Singleton with the given service and mapping to the specified implementation <see cref="Type" />.
        /// </summary>
        /// <param name="from">The service <see cref="Type" /></param>
        /// <param name="to">The implementation <see cref="Type" /></param>
        /// <param name="name">The name or key to register the service</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleSingleton().Named(name));
            return this;
        }

        /// <summary>
        /// Registers a Singleton with the given service <see cref="Type" /> factory delegate method.
        /// </summary>
        /// <param name="type">The service <see cref="Type" /></param>
        /// <param name="factoryMethod">The delegate method.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            Instance.Register(Component.For(type).UsingFactoryMethod(factoryMethod).LifestyleSingleton());
            return this;
        }


        /// <summary>
        /// Registers a Singleton with the given service <see cref="Type" /> factory delegate method.
        /// </summary>
        /// <param name="type">The service <see cref="Type" /></param>
        /// <param name="factoryMethod">The delegate method using <see cref="IContainerProvider"/>.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.Register(Component.For(type).UsingFactoryMethod(x => factoryMethod(x.Resolve<IContainerProvider>())).LifestyleSingleton());
            return this;
        }


        /// <summary>
        /// Registers a Singleton Service which implements service interfaces
        /// </summary>
        /// <param name="type">The implementation <see cref="Type" />.</param>
        /// <param name="serviceTypes">The service <see cref="Type"/>'s.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        /// <remarks>Registers all interfaces if none are specified.</remarks>
        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {
            Instance.Register(Component.For(type).LifestyleSingleton());
            return RegisterManyInternal(type, serviceTypes);
        }

        private IContainerRegistry RegisterManyInternal(Type implementingType, Type[] serviceTypes)
        {
            if (serviceTypes is null || serviceTypes.Length == 0)
            {
                serviceTypes = implementingType.GetInterfaces().Where(x => x != typeof(IDisposable)).ToArray();
            }

            foreach (var service in serviceTypes)
            {
                Instance.Register(Component.For(service).UsingFactoryMethod(x => x.Resolve(implementingType)));
            }
            return this;
        }

        /// <summary>
        /// Registers a Transient with the given service and mapping to the specified implementation <see cref="Type" />.
        /// </summary>
        /// <param name="from">The service <see cref="Type" /></param>
        /// <param name="to">The implementation <see cref="Type" /></param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleTransient());
            return this;
        }
        /// <summary>
        /// Registers a Transient with the given service and mapping to the specified implementation <see cref="Type" />.
        /// </summary>
        /// <param name="from">The service <see cref="Type" /></param>
        /// <param name="to">The implementation <see cref="Type" /></param>
        /// <param name="name">The name or key to register the service</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).Named(name).LifestyleTransient());
            return this;
        }

        /// <summary>
        /// Registers a Transient Service using a delegate method
        /// </summary>
        /// <param name="type">The service <see cref="Type" /></param>
        /// <param name="factoryMethod">The delegate method.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            Instance.Register(Component.For(type).UsingFactoryMethod(factoryMethod));
            return this;
        }

        /// <summary>
        /// Registers a Transient Service using a delegate method
        /// </summary>
        /// <param name="type">The service <see cref="Type" /></param>
        /// <param name="factoryMethod">The delegate method using <see cref="IContainerProvider"/>.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.Register(Component.For(type).UsingFactoryMethod(x => factoryMethod(x.Resolve<IContainerProvider>())).LifestyleSingleton());
            return this;
        }

        /// <summary>
        /// Registers a Transient Service which implements service interfaces
        /// </summary>
        /// <param name="type">The implementing <see cref="Type" />.</param>
        /// <param name="serviceTypes">The service <see cref="Type"/>'s.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        /// <remarks>Registers all interfaces if none are specified.</remarks>
        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            Instance.Register(Component.For(type));
            return RegisterManyInternal(type, serviceTypes);
        }

        /// <summary>
        /// Registers a scoped service
        /// </summary>
        /// <param name="from">The service <see cref="Type" /></param>
        /// <param name="to">The implementation <see cref="Type" /></param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterScoped(Type from, Type to)
        {
            Instance.Register(Component.For(from).ImplementedBy(to).LifestyleScoped());
            return this;
        }

        /// <summary>
        /// Registers a scoped service using a delegate method.
        /// </summary>
        /// <param name="type">The service <see cref="Type" /></param>
        /// <param name="factoryMethod">The delegate method.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            Instance.Register(Component.For(type).UsingFactoryMethod(factoryMethod).LifestyleScoped());
            return this;
        }

        /// <summary>
        /// Registers a scoped service using a delegate method.
        /// </summary>
        /// <param name="type">The service <see cref="Type"/>.</param>
        /// <param name="factoryMethod">The delegate method.</param>
        /// <returns>The <see cref="IContainerRegistry" /> instance</returns>
        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.Register(Component.For(type).UsingFactoryMethod(c => factoryMethod(c.Resolve<IContainerProvider>())).LifestyleScoped());
            return this;
        }

        /// <summary>
        /// Resolves a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The service <see cref="Type"/></param>
        /// <returns>The resolved Service <see cref="Type"/></returns>

        public object Resolve(Type type) =>
            Resolve(type, Array.Empty<(Type, object)>());

        /// <summary>
        /// Resolves a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The service <see cref="Type"/></param>
        /// <param name="name">The service name/key used when registering the <see cref="Type"/></param>
        /// <returns>The resolved Service <see cref="Type"/></returns>
        public object Resolve(Type type, string name) =>
            Resolve(type, name, Array.Empty<(Type, object)>());

        /// <summary>
        /// Resolves a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The service <see cref="Type"/></param>
        /// <param name="parameters">Typed parameters to use when resolving the Service</param>
        /// <returns>The resolved Service <see cref="Type"/></returns>
        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            var container = _currentScope?.Container ?? Instance;
            var arguments = new Arguments
            {
                parameters.Select(p => new KeyValuePair<object, object>(p.Type, p.Instance))
            };
            return container.ResolveType(type);
        }


        /// <summary>
        /// Resolves a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The service <see cref="Type"/></param>
        /// <param name="name"></param>
        /// <param name="parameters">Typed parameters to use when resolving the Service</param>
        /// <returns>The resolved Service <see cref="Type"/></returns>
        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            var container = _currentScope?.Container ?? Instance;
            var arguments = new Arguments
            {
                parameters.Select(p => new KeyValuePair<object, object>(p.Type, p.Instance))
            };
            return container.ResolveType(name, type);
        }

        /// <summary>
        /// Determines if a given service is registered
        /// </summary>
        /// <param name="type">The service <see cref="Type" /></param>
        /// <returns><c>true</c> if the service is registered.</returns>
        public bool IsRegistered(Type type)
        {
            return Instance.Kernel.HasComponent(type);
        }

        /// <summary>
        /// Determines if a given service is registered with the specified name
        /// </summary>
        /// <param name="type">The service <see cref="Type" /></param>
        /// <param name="name">The service name or key used</param>
        /// <returns><c>true</c> if the service is registered.</returns>
        public bool IsRegistered(Type type, string name)
        {
            return Instance.Kernel.HasComponent(type);
        }
        Type IContainerInfo.GetRegistrationType(string key)
        {
            //First try friendly name registration. If not found, try type registration
            var matchingRegistration = Instance.Kernel.GetAssignableHandlers(typeof(object)).FirstOrDefault(x => x.GetComponentName().Equals(key, StringComparison.Ordinal));
            return matchingRegistration?.GetComponentType();
        }

        Type IContainerInfo.GetRegistrationType(Type serviceType)
        {
            //First try friendly name registration. If not found, try type registration
            var matchingRegistration = Instance.Kernel.GetAssignableHandlers(typeof(object)).FirstOrDefault(x => x.GetType() == serviceType);
            return matchingRegistration?.GetComponentType();
        }

        /// <summary>
        /// Creates a new Scope
        /// </summary>
        public virtual IScopedProvider CreateScope() =>
            CreateScopeInternal();


        /// <summary>
        /// Creates a new Scope and provides the updated ServiceProvider
        /// </summary>
        /// <returns>A child <see cref="IWindsorContainer" />.</returns>
        /// <remarks>
        /// This should be called by custom implementations that Implement IServiceScopeFactory
        /// </remarks>
        protected IScopedProvider CreateScopeInternal()
        {
            var childContainer = new WindsorContainer();
            Instance.AddChildContainer(childContainer);
            _currentScope = new WindsorScopedProvider(childContainer);
            return _currentScope;
        }

        private class WindsorScopedProvider : IScopedProvider
        {
            public WindsorScopedProvider(IWindsorContainer container)
            {
                Container = container;
            }

            public IWindsorContainer Container { get; private set; }
            public bool IsAttached { get; set; }
            public IScopedProvider CurrentScope => this;

            public IScopedProvider CreateScope() => this;

            public void Dispose()
            {
                Container.Dispose();
                Container = null;
            }

            public object Resolve(Type type) =>
                Resolve(type, Array.Empty<(Type, object)>());

            public object Resolve(Type type, string name) =>
                Resolve(type, name, Array.Empty<(Type, object)>());

            public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    var arguments = new Arguments
                    {
                        parameters.Select(p => new KeyValuePair<object, object>(p.Type, p.Instance))
                    };
                    return Container.Resolve(type, arguments);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, ex);
                }
            }

            public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    // Windsor will simply return a new object() for unregistered Views
                    var arguments = new Arguments
                    {
                        parameters.Select(p => new KeyValuePair<object, object>(p.Type, p.Instance))
                    };
                    return Container.Resolve(name, type, arguments);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, name, ex);
                }
            }
        }

        /// <summary>
        /// Used to perform any final steps for configuring the extension that may be required by the container.
        /// </summary>
        public void FinalizeExtension()
        {

        }
    }
}
