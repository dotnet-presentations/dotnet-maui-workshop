using System;
using System.ComponentModel;

namespace Caliburn.Light {
    /// <summary>
    /// Extends <see cref="INotifyPropertyChanged"/> such that the change event can be suspended.
    /// </summary>
    public interface IBindableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Suspends property change notifications.
        /// </summary>
        IDisposable SuspendNotifications();

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        void Refresh();
    }
}
