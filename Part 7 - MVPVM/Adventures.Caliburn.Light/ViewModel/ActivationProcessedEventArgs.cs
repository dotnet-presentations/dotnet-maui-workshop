using System;

namespace Caliburn.Light
{
    /// <summary>
    /// Contains details about the success or failure of an item's activation through an <see cref="IConductor"/>.
    /// </summary>
    public sealed class ActivationProcessedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationProcessedEventArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        public ActivationProcessedEventArgs(object item, bool success)
        {
            Item = item;
            Success = success;
        }

        /// <summary>
        /// The item whose activation was processed.
        /// </summary>
        public object Item { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the activation was a success.
        /// </summary>
        public bool Success { get; }
    }
}
