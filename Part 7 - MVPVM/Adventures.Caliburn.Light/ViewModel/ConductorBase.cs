using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// A base class for various implementations of <see cref="IConductor"/>.
    /// </summary>
    /// <typeparam name="T">The type that is being conducted.</typeparam>
    public abstract class ConductorBase<T> : Screen, IConductor where T : class
    {
        private static readonly DefaultCloseStrategy<T> _defaultCloseStrategy = new DefaultCloseStrategy<T>();
        private ICloseStrategy<T> _closeStrategy;

        /// <summary>
        /// Gets or sets the close strategy.
        /// </summary>
        /// <value>The close strategy.</value>
        public ICloseStrategy<T> CloseStrategy
        {
            get { return _closeStrategy ?? _defaultCloseStrategy; }
            set { _closeStrategy = value; }
        }

        Task IConductor.ActivateItemAsync(object item)
        {
            return ActivateItemAsync((T) item);
        }

        Task IConductor.DeactivateItemAsync(object item, bool close)
        {
            return DeactivateItemAsync((T) item, close);
        }

        IEnumerable IConductor.GetChildren()
        {
            return GetChildren();
        }

        /// <summary>
        /// Occurs when an activation request is processed.
        /// </summary>
        public event EventHandler<ActivationProcessedEventArgs> ActivationProcessed;

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns>The collection of children.</returns>
        public abstract IReadOnlyList<T> GetChildren();

        /// <summary>
        /// Activates the specified item.
        /// </summary>
        /// <param name="item">The item to activate.</param>
        public abstract Task ActivateItemAsync(T item);

        /// <summary>
        /// Deactivates the specified item.
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        public abstract Task DeactivateItemAsync(T item, bool close);

        /// <summary>
        /// Called by a subclass when an activation needs processing.
        /// </summary>
        /// <param name="item">The item on which activation was attempted.</param>
        /// <param name="success">if set to <c>true</c> activation was successful.</param>
        protected virtual void OnActivationProcessed(T item, bool success)
        {
            ActivationProcessed?.Invoke(this, new ActivationProcessedEventArgs(item, success));
        }

        /// <summary>
        /// Ensures that an item is ready to be activated.
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns>The item to be activated.</returns>
        protected virtual T EnsureItem(T newItem)
        {
            if (newItem is IChild node && node.Parent != this)
                node.Parent = this;

            return newItem;
        }
    }
}
