using System.Threading.Tasks;

namespace Caliburn.Light
{
    /// <summary>
    /// Denotes an instance which may prevent closing.
    /// </summary>
    public interface ICloseGuard : IClose
    {
        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <returns>A task containing the result of the close check.</returns>
        Task<bool> CanCloseAsync();
    }
}
