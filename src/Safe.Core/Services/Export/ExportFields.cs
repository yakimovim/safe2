namespace Safe.Core.Services.Export
{
    public abstract class ExportField
    {
        /// <summary>
        /// Label of the field.
        /// </summary>
        public string Name { get; set; }
    }

    public sealed class ExportTextField : ExportField
    {
        /// <summary>
        /// Text value.
        /// </summary>
        public string Text { get; set; }
    }

    public sealed class ExportPasswordField : ExportField
    {
        /// <summary>
        /// Password value.
        /// </summary>
        public string Password { get; set; }
    }
}