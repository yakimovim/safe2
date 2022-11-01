namespace Safe.Core.Services.Export
{
    public class ExportItem
    {
        /// <summary>
        /// Title of the item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of tags for this item.
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// List of fields of this item.
        /// </summary>
        public ExportField[] Fields { get; set; }

    }
}