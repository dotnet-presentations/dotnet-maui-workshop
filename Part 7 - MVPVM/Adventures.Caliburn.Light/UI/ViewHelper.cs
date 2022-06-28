using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// The helper a view-model uses to interact with the view.
    /// </summary>
    public static class ViewHelper
    {
        private static readonly List<IViewAdapter> _viewAdapters = new List<IViewAdapter>();

        /// <summary>
        /// Gets whether the <see cref="ViewHelper"/> is initialized.
        /// </summary>
        public static bool IsInitialized => _viewAdapters.Count > 0;

        /// <summary>
        /// Initializes the <see cref="ViewHelper"/>.
        /// </summary>
        /// <param name="viewAdapter">The adapter to interact with view elements.</param>
        public static void Initialize(IViewAdapter viewAdapter)
        {
            if (_viewAdapters.Contains(viewAdapter))
                return;

            _viewAdapters.Add(viewAdapter);
        }

        /// <summary>
        /// Indicates whether or not the framework is running in the context of a designer.
        /// </summary>
        public static bool IsInDesignTool
        {
            get { return _viewAdapters.Count == 0 || _viewAdapters.Exists(v => v.IsInDesignTool); }
        }

        private static IViewAdapter ResolveViewAdapter(object view)
        {
            var viewAdapter = _viewAdapters.Find(v => v.CanHandle(view));
            return viewAdapter ?? ThrowNotInitializedException(view);
        }

        private static IViewAdapter ThrowNotInitializedException(object view)
        {
            throw new InvalidOperationException("No view adapter for " + view.GetType() + " is initialized.");
        }

        /// <summary>
        /// Used to retrieve the root, non-framework-created view.
        /// </summary>
        /// <param name="view">The view to search.</param>
        /// <returns>The root element that was not created by the framework.</returns>
        public static object GetFirstNonGeneratedView(object view)
        {
            return ResolveViewAdapter(view).GetFirstNonGeneratedView(view);
        }

        /// <summary>
        /// Executes the handler the fist time the view is loaded.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public static void ExecuteOnFirstLoad(object view, Action<object> handler)
        {
            ResolveViewAdapter(view).ExecuteOnFirstLoad(view, handler);
        }

        /// <summary>
        /// Executes the handler the next time the view's LayoutUpdated event fires.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public static void ExecuteOnLayoutUpdated(object view, Action<object> handler)
        {
            ResolveViewAdapter(view).ExecuteOnLayoutUpdated(view, handler);
        }

        /// <summary>
        /// Tries to close the specified view.
        /// </summary>
        /// <param name="view">The view to close.</param>
        /// <returns>true, when close could be initiated; otherwise false.</returns>
        public static Task<bool> TryCloseAsync(object view)
        {
            return ResolveViewAdapter(view).TryCloseAsync(view);
        }

        /// <summary>
        /// Gets the command parameter of the view.
        /// This can be the native CommandParameter property or 'cal:View.CommandParameter'.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The command parameter.</returns>
        public static object GetCommandParameter(object view)
        {
            return ResolveViewAdapter(view).GetCommandParameter(view);
        }

        /// <summary>
        /// Gets the Dispatcher this view is associated with.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The dispatcher for the view.</returns>
        public static IDispatcher GetDispatcher(object view)
        {
            return ResolveViewAdapter(view).GetDispatcher(view);
        }
    }
}
