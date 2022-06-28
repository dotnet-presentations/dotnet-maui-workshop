using System;
using System.Collections;
using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Denotes an instance which conducts other objects by maintaining a strict life-cycle.
    /// </summary>
    /// <remarks>
    /// Conducted instances can opt-in to the life-cycle by implementing any of the following
    /// <see cref="IActivatable"/>, <see cref="ICloseGuard"/>, <see cref="IChild"/>.
    /// </remarks>
    public interface IConductor
    {
        /// <summary>
        /// Activates the specified item.
        /// </summary>
        /// <param name="item">The item to activate.</param>
        Task ActivateItemAsync(object item);

        /// <summary>
        /// Deactivates the specified item.
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        Task DeactivateItemAsync(object item, bool close);

        /// <summary>
        /// Occurs when an activation request is processed.
        /// </summary>
        event EventHandler<ActivationProcessedEventArgs> ActivationProcessed;

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns>The collection of children.</returns>
        IEnumerable GetChildren();
    }
}
