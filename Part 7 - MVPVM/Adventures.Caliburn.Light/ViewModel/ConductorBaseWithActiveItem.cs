using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// A base class for various implementations of <see cref="IConductor"/> that maintain an active item.
    /// </summary>
    /// <typeparam name="T">The type that is being conducted.</typeparam>
    public abstract class ConductorBaseWithActiveItem<T> : ConductorBase<T>, IHaveActiveItem where T : class
    {
        private T _activeItem;

        /// <summary>
        /// The currently active item.
        /// </summary>
        public T ActiveItem
        {
            get { return _activeItem; }
            set { ActivateItemAsync(value).Observe(); }
        }

        object IHaveActiveItem.ActiveItem => ActiveItem;

        /// <summary>
        /// Changes the active item.
        /// </summary>
        /// <param name="newItem">The new item to activate.</param>
        /// <param name="closePrevious">Indicates whether or not to close the previous active item.</param>
        protected async Task ChangeActiveItemAsync(T newItem, bool closePrevious)
        {
            if (ActiveItem is IActivatable deactivator)
                await deactivator.DeactivateAsync(closePrevious);

            if (newItem is not null)
                newItem = EnsureItem(newItem);

            if (IsActive && newItem is IActivatable activator)
                await activator.ActivateAsync();

            SetProperty(ref _activeItem, newItem, nameof(ActiveItem));

            if (newItem is not null)
                OnActivationProcessed(newItem, true);
        }
    }
}
