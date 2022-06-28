using System;

namespace Caliburn.Light
{
    /// <summary>
    /// The event arguments for the <see cref="IViewAware.ViewAttached"/> event.
    /// </summary>
    public sealed class ViewAttachedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewAttachedEventArgs"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context.</param>
        public ViewAttachedEventArgs(object view, object context)
        {
            View = view;
            Context = context;
        }

        /// <summary>
        /// The view.
        /// </summary>
        public object View { get; }

        /// <summary>
        /// The context.
        /// </summary>
        public object Context { get; }
    }
}
