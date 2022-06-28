namespace Caliburn.Light
{
    /// <summary>
    /// Denotes a node within a parent/child hierarchy.
    /// </summary>
    public interface IChild
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        object Parent { get; set; }
    }
}
