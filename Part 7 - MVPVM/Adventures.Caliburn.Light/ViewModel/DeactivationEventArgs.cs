using System;

namespace Caliburn.Light
{
    /// <summary>
    /// EventArgs sent during deactivation.
    /// </summary>
    public sealed class DeactivationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeactivationEventArgs"/> class.
        /// </summary>
        /// <param name="wasClosed">if set to <c>true</c> [was closed].</param>
        public DeactivationEventArgs(bool wasClosed)
        {
            WasClosed = wasClosed;
        }

        /// <summary>
        /// Indicates whether the sender was closed in addition to being deactivated.
        /// </summary>
        public bool WasClosed { get; }
    }
}
