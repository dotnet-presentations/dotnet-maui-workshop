using System;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// The interface a view-model uses to interact with the view.
    /// </summary>
    public interface IViewAdapter
    {
        /// <summary>
        /// Indicates whether or not the framework is running in the context of a designer.
        /// </summary>
        bool IsInDesignTool { get; }

        /// <summary>
        /// Determines whether this instance can handle the provided view type.
        /// </summary>
        /// <param name="view">The view to verify.</param>
        /// <returns>True when this instance can handle the provided view; otherwise false.</returns>
        bool CanHandle(object view);

        /// <summary>
        /// Used to retrieve the root, non-framework-created view.
        /// </summary>
        /// <param name="view">The view to search.</param>
        /// <returns>The root element that was not created by the framework.</returns>
        object GetFirstNonGeneratedView(object view);

        /// <summary>
        /// Executes the handler the fist time the view is loaded.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        void ExecuteOnFirstLoad(object view, Action<object> handler);

        /// <summary>
        /// Executes the handler the next time the view's LayoutUpdated event fires.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        void ExecuteOnLayoutUpdated(object view, Action<object> handler);

        /// <summary>
        /// Tries to close the specified view.
        /// </summary>
        /// <param name="view">The view to close.</param>
        /// <returns>true, when close could be initiated; otherwise false.</returns>
        Task<bool> TryCloseAsync(object view);

        /// <summary>
        /// Gets the command parameter of the view.
        /// This can be the native CommandParameter property or 'cal:View.CommandParameter'.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The command parameter.</returns>
        object GetCommandParameter(object view);

        /// <summary>
        /// Gets the Dispatcher this view is associated with.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The dispatcher for the view.</returns>
        IDispatcher GetDispatcher(object view);
    }
}
