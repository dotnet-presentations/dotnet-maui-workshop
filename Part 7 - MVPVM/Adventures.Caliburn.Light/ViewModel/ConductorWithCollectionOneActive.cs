using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    public partial class Conductor<T>
    {
        /// <summary>
        /// An implementation of <see cref="IConductor"/> that holds on many items.
        /// </summary>
        public static partial class Collection
        {
            /// <summary>
            /// An implementation of <see cref="IConductor"/> that holds on many items but only activates one at a time.
            /// </summary>
            public class OneActive : ConductorBaseWithActiveItem<T>
            {
                private readonly BindableCollection<T> _items = new BindableCollection<T>();

                /// <summary>
                /// Initializes a new instance of <see cref="Conductor&lt;T&gt;.Collection.OneActive"/>.
                /// </summary>
                public OneActive()
                {
                    _items.AreChildrenOf(this);
                }

                /// <summary>
                /// Gets the items that are currently being conducted.
                /// </summary>
                public IBindableCollection<T> Items => _items;

                /// <summary>
                /// Gets the children.
                /// </summary>
                /// <returns>The collection of children.</returns>
                public sealed override IReadOnlyList<T> GetChildren() => _items;

                /// <summary>
                /// Activates the specified item.
                /// </summary>
                /// <param name="item">The item to activate.</param>
                public override async Task ActivateItemAsync(T item)
                {
                    if (item is null)
                    {
                        await DeactivateItemAsync(ActiveItem, false);
                        return;
                    }

                    var isActiveItem = ReferenceEquals(item, ActiveItem);
                    if (isActiveItem)
                    {
                        if (IsActive)
                        {
                            if (item is IActivatable activeItem)
                                await activeItem.ActivateAsync();

                            OnActivationProcessed(item, true);
                        }

                        return;
                    }

                    await ChangeActiveItemAsync(item, false);
                }

                /// <summary>
                /// Deactivates the specified item.
                /// </summary>
                /// <param name="item">The item to close.</param>
                /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
                public override async Task DeactivateItemAsync(T item, bool close)
                {
                    if (item is null)
                        return;

                    if (!close)
                    {
                        if (item is IActivatable deactivator)
                            await deactivator.DeactivateAsync(false);

                        return;
                    }

                    var result = await CloseStrategy.ExecuteAsync(new[] { item });
                    if (result.CanClose)
                    {
                        if (!ReferenceEquals(item, ActiveItem))
                        {
                            if (item is IActivatable deactivator)
                                await deactivator.DeactivateAsync(true);
                        }
                        else
                        {
                            var index = _items.IndexOf(item);
                            var next = DetermineNextItemToActivate(_items, index);
                            await ChangeActiveItemAsync(next, true);
                        }

                        _items.Remove(item);
                    }
                }

                /// <summary>
                /// Determines the next item to activate based on the last active index.
                /// </summary>
                /// <param name="list">The list of possible active items.</param>
                /// <param name="lastIndex">The index of the last active item.</param>
                /// <returns>The next item to activate.</returns>
                /// <remarks>Called after an active item is closed.</remarks>
                protected virtual T DetermineNextItemToActivate(IList<T> list, int lastIndex)
                {
                    var toRemoveAt = lastIndex - 1;

                    if (toRemoveAt == -1 && list.Count > 1)
                        return list[1];

                    if (toRemoveAt > -1 && toRemoveAt < list.Count - 1)
                        return list[toRemoveAt];

                    return default;
                }

                /// <summary>
                /// Called to check whether or not this instance can close.
                /// </summary>
                /// <returns>A task containing the result of the close check.</returns>
                public override async Task<bool> CanCloseAsync()
                {
                    var result = await CloseStrategy.ExecuteAsync(_items.ToArray());

                    var canClose = result.CanClose;
                    var closables = result.Closeables;

                    if (!canClose && closables.Count > 0)
                    {
                        if (closables.Contains(ActiveItem))
                        {
                            var list = _items.ToList();
                            var next = ActiveItem;
                            do
                            {
                                var previous = next;
                                next = DetermineNextItemToActivate(list, list.IndexOf(previous));
                                list.Remove(previous);
                            } while (closables.Contains(next));

                            var previousActive = ActiveItem;
                            await ChangeActiveItemAsync(next, true);
                            _items.Remove(previousActive);

                            var stillToClose = closables.ToList();
                            stillToClose.Remove(previousActive);
                            closables = stillToClose;
                        }

                        await Task.WhenAll(closables
                            .OfType<IActivatable>()
                            .Select(x => x.DeactivateAsync(true)));

                        _items.RemoveRange(closables);
                    }

                    return canClose;
                }

                /// <summary>
                /// Called when activating.
                /// </summary>
                protected override async Task OnActivateAsync()
                {
                    if (ActiveItem is IActivatable activator)
                        await activator.ActivateAsync();
                }

                /// <summary>
                /// Called when deactivating.
                /// </summary>
                /// <param name="close">Indicates whether this instance will be closed.</param>
                protected override async Task OnDeactivateAsync(bool close)
                {
                    if (close)
                    {
                        await Task.WhenAll(_items
                            .OfType<IActivatable>()
                            .Select(x => x.DeactivateAsync(true)));

                        _items.Clear();
                    }
                    else
                    {
                        if (ActiveItem is IActivatable deactivator)
                            await deactivator.DeactivateAsync(false);
                    }
                }

                /// <summary>
                /// Ensures that an item is ready to be activated.
                /// </summary>
                /// <param name="newItem"></param>
                /// <returns>The item to be activated.</returns>
                protected override T EnsureItem(T newItem)
                {
                    if (newItem is null)
                    {
                        newItem = DetermineNextItemToActivate(_items, ActiveItem is not null ? _items.IndexOf(ActiveItem) : 0);
                    }
                    else
                    {
                        var index = _items.IndexOf(newItem);

                        if (index < 0)
                            _items.Add(newItem);
                        else
                            newItem = _items[index];
                    }

                    return base.EnsureItem(newItem);
                }
            }
        }
    }
}
