using System;
using System.Windows.Controls;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Prism.CastleWindsor.Extensions
{
    public static class IWindsorContainerRegistryExtensions
    {
        /// <summary>
        /// Registers an object to be used as a dialog in the IDialogService.
        /// </summary>
        /// <typeparam name="TView">The Type of object to register as the dialog</typeparam>
        /// <param name="containerRegistry"></param>
        /// <param name="name">The unique name to register with the dialog.</param>
        public static void RegisterWindsorDialog<TView>(this IContainerRegistry containerRegistry, string name = null)
        {
            containerRegistry.RegisterForWindsorNavigation<TView>(name);
        }

        /// <summary>
        /// Registers an object to be used as a dialog in the IDialogService.
        /// </summary>
        /// <typeparam name="TView">The Type of object to register as the dialog</typeparam>
        /// <typeparam name="TViewModel">The ViewModel to use as the DataContext for the dialog</typeparam>
        /// <param name="containerRegistry"></param>
        /// <param name="name">The unique name to register with the dialog.</param>
        public static void RegisterWindsorDialog<TView, TViewModel>(this IContainerRegistry containerRegistry, string name = null) where TViewModel : IDialogAware
        {
            containerRegistry.RegisterForWindsorNavigation<TView, TViewModel>(name);
        }

        /// <summary>
        /// Registers an object that implements IDialogWindow to be used to host all dialogs in the IDialogService.
        /// </summary>
        /// <typeparam name="TWindow">The Type of the Window class that will be used to host dialogs in the IDialogService</typeparam>
        /// <param name="containerRegistry"></param>
        public static void RegisterWindsorDialogWindow<TWindow>(this IContainerRegistry containerRegistry) where TWindow : IDialogWindow
        {
            containerRegistry.Register(typeof(IDialogWindow), typeof(TWindow));
        }

        /// <summary>
        /// Registers an object that implements IDialogWindow to be used to host all dialogs in the IDialogService.
        /// </summary>
        /// <typeparam name="TWindow">The Type of the Window class that will be used to host dialogs in the IDialogService</typeparam>
        /// <param name="containerRegistry"></param>
        /// <param name="name">The name of the dialog window</param>
        public static void RegisterWindsorDialogWindow<TWindow>(this IContainerRegistry containerRegistry, string name) where TWindow : IDialogWindow
        {
            containerRegistry.Register(typeof(IDialogWindow), typeof(TWindow), name);
        }

        /// <summary>
        /// Registers an object for navigation
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// <param name="type">The type of object to register</param>
        /// <param name="name">The unique name to register with the object.</param>
        public static void RegisterForWindsorNavigation(this IContainerRegistry containerRegistry, Type type, string name)
        {
            containerRegistry.Register(typeof(UserControl), type, name);
        }

        /// <summary>
        /// Registers an object for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of the object to register as the view</typeparam>
        /// <param name="containerRegistry"></param>
        /// <param name="name">The unique name to register with the object.</param>
        public static void RegisterForWindsorNavigation<T>(this IContainerRegistry containerRegistry, string name = null)
        {
            Type type = typeof(T);
            string viewName = string.IsNullOrWhiteSpace(name) ? type.Name : name;
            containerRegistry.RegisterForWindsorNavigation(type, viewName);
        }

        /// <summary>
        /// Registers an object for navigation with the ViewModel type to be used as the DataContext.
        /// </summary>
        /// <typeparam name="TView">The Type of object to register as the view</typeparam>
        /// <typeparam name="TViewModel">The ViewModel to use as the DataContext for the view</typeparam>
        /// <param name="containerRegistry"></param>
        /// <param name="name">The unique name to register with the view</param>
        public static void RegisterForWindsorNavigation<TView, TViewModel>(this IContainerRegistry containerRegistry, string name = null)
        {
            containerRegistry.RegisterForWindsorNavigationWithViewModel<TViewModel>(typeof(TView), name);
        }

        private static void RegisterForWindsorNavigationWithViewModel<TViewModel>(this IContainerRegistry containerRegistry, Type viewType, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = viewType.Name;

            ViewModelLocationProvider.Register(viewType.ToString(), typeof(TViewModel));
            containerRegistry.RegisterForWindsorNavigation(viewType, name);
        }
    }
}
