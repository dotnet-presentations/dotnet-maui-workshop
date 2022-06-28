using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Used to gather the results from multiple child elements which may or may not prevent closing.
    /// </summary>
    /// <typeparam name="T">The type of child element.</typeparam>
    public sealed class DefaultCloseStrategy<T> : ICloseStrategy<T> where T : class
    {
        private readonly bool _closeConductedItemsWhenConductorCannotClose;

        /// <summary>
        /// Creates an instance of the class.
        /// </summary>
        /// <param name="closeConductedItemsWhenConductorCannotClose">Indicates that even if all conducted items are not closable, those that are should be closed.</param>
        public DefaultCloseStrategy(bool closeConductedItemsWhenConductorCannotClose = false)
        {
            _closeConductedItemsWhenConductorCannotClose = closeConductedItemsWhenConductorCannotClose;
        }

        /// <summary>
        /// Executes the strategy.
        /// </summary>
        /// <param name="toClose">Items that are requesting close.</param>
        /// <returns>A task containing the aggregated close results.</returns>
        public Task<CloseResult<T>> ExecuteAsync(IReadOnlyList<T> toClose)
        {
            return _closeConductedItemsWhenConductorCannotClose
                ? CanCloseAsyncWithClosables(toClose)
                : CanCloseAsync(toClose);
        }

        private static async Task<CloseResult<T>> CanCloseAsyncWithClosables(IReadOnlyList<T> toClose)
        {
            var closables = new List<T>();
            var results = await Task.WhenAll(toClose
                .Select(x => CanCloseItemAsync(x, closables)));

            return new CloseResult<T>(Array.TrueForAll(results, x => x), closables);
        }

        private static async Task<bool> CanCloseItemAsync(T item, List<T> closables)
        {
            if (item is ICloseGuard guard)
            {
                var canClose = await guard.CanCloseAsync();
                if (canClose)
                    closables.Add(item);

                return canClose;
            }
            else
            {
                closables.Add(item);
                return true;
            }
        }

        private static async Task<CloseResult<T>> CanCloseAsync(IReadOnlyList<T> toClose)
        {
            var results = await Task.WhenAll(toClose
                .OfType<ICloseGuard>()
                .Select(x => x.CanCloseAsync()));

            var result = Array.TrueForAll(results, x => x);
            return new CloseResult<T>(result, Array.Empty<T>());
        }
    }
}
