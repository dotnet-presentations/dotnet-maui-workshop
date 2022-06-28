using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Caliburn.Light
{
    /// <summary>
    /// A base class for objects of which the properties must be observable.
    /// </summary>
    public class BindableObject : IBindableObject, INotifyPropertyChanging
    {
        private int _suspensionCount;

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Suspends the notifications.
        /// </summary>
        /// <returns></returns>
        public IDisposable SuspendNotifications()
        {
            _suspensionCount++;
            return new DisposableAction(ResumeNotifications);
        }

        private void ResumeNotifications()
        {
            _suspensionCount--;
        }

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        public void Refresh()
        {
            RaisePropertyChanging(string.Empty);
            RaisePropertyChanged(string.Empty);
        }

        /// <summary>
        /// Determines whether notifications are suspended.
        /// </summary>
        /// <returns></returns>
        protected bool AreNotificationsSuspended()
        {
            return _suspensionCount > 0;
        }

        /// <summary>
        /// Raises the PropertyChanging event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that is changing.</param>
        protected void RaisePropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (AreNotificationsSuspended()) return;
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (AreNotificationsSuspended()) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Assigns a new value to the property. Then, raises the PropertyChanged event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <returns>True if the PropertyChanged event has been raised, false otherwise.
        /// The event is not raised if the old value is equal to the new value.</returns>
        protected virtual bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;

            RaisePropertyChanging(propertyName);
            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
