using System;

namespace Caliburn.Light
{
    /// <summary>
    /// EventArgs sent during activation.
    /// </summary>
    public sealed class ActivationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationEventArgs"/> class.
        /// </summary>
        /// <param name="wasInitialized">if the sender was initialized.</param>
        public ActivationEventArgs(bool wasInitialized)
        {
            WasInitialized = wasInitialized;
        }

        /// <summary>
        /// Indicates whether the sender was initialized in addition to being activated.
        /// </summary>
        public bool WasInitialized { get; }
    }
}
