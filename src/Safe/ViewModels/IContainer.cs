namespace Safe.ViewModels
{
    /// <summary>
    /// Represents container of many items.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    public interface IContainer<in T>
    {
        /// <summary>
        /// Adds <paramref name="item"/> into the container.
        /// </summary>
        /// <param name="item">Item.</param>
        void Add(T item);
        /// <summary>
        /// Deletes <paramref name="item"/> from the container.
        /// </summary>
        /// <param name="item">Item.</param>
        void Delete(T item);
        /// <summary>
        /// Checks if <paramref name="item"/> can be moved up in the container.
        /// </summary>
        /// <param name="item">Item.</param>
        bool CanMoveUp(T item);
        /// <summary>
        /// Checks if <paramref name="item"/> can be moved down in the container.
        /// </summary>
        /// <param name="item">Item.</param>
        bool CanMoveDown(T item);
        /// <summary>
        /// Moves <paramref name="item"/> up in the container.
        /// </summary>
        /// <param name="item">Item.</param>
        void MoveUp(T item);
        /// <summary>
        /// Moves <paramref name="item"/> down in the container.
        /// </summary>
        /// <param name="item">Item.</param>
        void MoveDown(T item);
    }
}
