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
            /// An implementation of <see cref="IConductor"/> that holds on to many items which are all activated.
            /// </summary>
            public class AllActive : ConductorBase<T>
            {
                private readonly BindableCollection<T> _items = new BindableCollection<T>();

                /// <summary>
                /// Initializes a new instance of <see cref="Conductor&lt;T&gt;.Collection.AllActive"/>.
                /// </summary>
                public AllActive()
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
                /// Called when activating.
                /// </summary>
                protected override Task OnActivateAsync()
                {
                    return Task.WhenAll(_items
                        .OfType<IActivatable>()
                        .Select(x => x.ActivateAsync()));
                }

                /// <summary>
                /// Called when deactivating.
                /// </summary>
                /// <param name="close">Indicates whether this instance will be closed.</param>
                protected override async Task OnDeactivateAsync(bool close)
                {
                    await Task.WhenAll(_items
                        .OfType<IActivatable>()
                        .Select(x => x.DeactivateAsync(close)));

                    if (close)
                        _items.Clear();
                }

                /// <summary>
                /// Called to check whether or not this instance can close.
                /// </summary>
                /// <returns>A task containing the result of the close check.</returns>
                public override async Task<bool> CanCloseAsync()
                {
                    var result = await CloseStrategy.ExecuteAsync(_items.ToArray());

                    if (!result.CanClose && result.Closeables.Count > 0)
                    {
                        await Task.WhenAll(result.Closeables.OfType<IActivatable>()
                            .Select(x => x.DeactivateAsync(true)));

                        _items.RemoveRange(result.Closeables);
                    }

                    return result.CanClose;
                }

                /// <summary>
                /// Activates the specified item.
                /// </summary>
                /// <param name="item">The item to activate.</param>
                public override async Task ActivateItemAsync(T item)
                {
                    if (item is null)
                        return;

                    item = EnsureItem(item);

                    if (IsActive && item is IActivatable activator)
                        await activator.ActivateAsync();

                    OnActivationProcessed(item, true);
                    return;
                }

                /// <summary>
                /// Deactivates the specified item.
                /// </summary>
                /// <param name="item">The item to close.</param>
                /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
                public override async Task DeactivateItemAsync(T item, bool close)
                {
                    if (item is null || !close)
                        return;

                    var result = await CloseStrategy.ExecuteAsync(new[] { item });
                    if (result.CanClose)
                    {
                        if (item is IActivatable deactivator)
                            await deactivator.DeactivateAsync(true);

                        _items.Remove(item);
                    }
                }

                /// <summary>
                /// Ensures that an item is ready to be activated.
                /// </summary>
                /// <param name="newItem"></param>
                /// <returns>The item to be activated.</returns>
                protected override T EnsureItem(T newItem)
                {
                    var index = _items.IndexOf(newItem);

                    if (index < 0)
                        _items.Add(newItem);
                    else
                        newItem = _items[index];

                    return base.EnsureItem(newItem);
                }
            }
        }
    }
}
