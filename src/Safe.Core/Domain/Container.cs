using System;

namespace Safe.Core.Domain
{
    /// <summary>
    /// Container of information for storage.
    /// </summary>
    [Serializable]
    public class Container
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public ListOfNonNullItems<Item> Items { get; } = new ListOfNonNullItems<Item>();
    }
}
