
namespace Caliburn.Light
{
    /// <summary>
    /// Denotes an object that has no fixed value, but can resolve it's value on runtime.
    /// </summary>
    public interface ISpecialValue
    {
        /// <summary>
        /// Resolves the value of this instance.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The resolved value.</returns>
        object Resolve(CommandExecutionContext context);
    }
}
