using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Caliburn.Light
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class BindableCollection<T> : ObservableCollection<T>, IBindableCollection<T>, IReadOnlyBindableCollection<T>
    {
        private int _suspensionCount;

        /// <summary>
        /// Initializes a new instance of BindableCollection that is empty and has default initial capacity.
        /// </summary>
        public BindableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the BindableCollection that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new BindableCollection.</param>
        public BindableCollection(IEnumerable<T> collection) : base(collection)
        {
        }

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
        /// Determines whether notifications are suspended.
        /// </summary>
        /// <returns></returns>
        protected bool AreNotificationsSuspended()
        {
            return _suspensionCount > 0;
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (AreNotificationsSuspended()) return;
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="INotifyCollectionChanged.CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (AreNotificationsSuspended()) return;
            base.OnCollectionChanged(e);
        }

        /// <summary>
        /// Raises a property and collection changed event that notifies that all of the properties on this object have changed.
        /// </summary>
        protected virtual void OnCollectionRefreshed()
        {
            if (AreNotificationsSuspended()) return;
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Raises a property and collection changed event that notifies that all of the properties on this object have changed.
        /// </summary>
        public void Refresh()
        {
            CheckReentrancy();
            OnCollectionRefreshed();
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the BindableCollection.
        /// </summary>
        /// <param name="items">The collection whose elements should be added to the end of the BindableCollection.</param>
        public void AddRange(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            CheckReentrancy();
            foreach (var item in items) { Items.Add(item); }
            OnCollectionRefreshed();
        }

        /// <summary>
        /// Removes a range of elements from the BindableCollection.
        /// </summary>
        /// <param name="items">The collection whose elements should be removed from the BindableCollection.</param>
        public void RemoveRange(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            CheckReentrancy();
            foreach (var item in items) { Items.Remove(item); }
            OnCollectionRefreshed();
        }
    }
}
