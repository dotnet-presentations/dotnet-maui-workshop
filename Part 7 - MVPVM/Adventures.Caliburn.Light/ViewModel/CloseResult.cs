using System.Collections.Generic;

namespace Caliburn.Light
{
    /// <summary>
    /// The result of an <see cref="ICloseStrategy&lt;Task&gt;"/>.
    /// </summary>
    /// <typeparam name="T">The type of child element.</typeparam>
    public readonly struct CloseResult<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CloseResult&lt;Task&gt;"/>
        /// </summary>
        /// <param name="canClose">Indicates whether close can occur.</param>
        /// <param name="closeables">Indicates which children should close if the parent cannot.</param>
        public CloseResult(bool canClose, IReadOnlyList<T> closeables)
        {
            CanClose = canClose;
            Closeables = closeables;
        }

        /// <summary>
        /// Indicates whether close can occur.
        /// </summary>
        public bool CanClose { get; }

        /// <summary>
        /// Indicates which children should close if the parent cannot.
        /// </summary>
        public IReadOnlyList<T> Closeables { get; }
    }
}
