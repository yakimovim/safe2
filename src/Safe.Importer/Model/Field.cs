namespace Safe.Importer.Model
{
    public enum FieldType
	{
		SingleLine,
		Password,
		Multiline
	}

	public sealed class FieldDto
	{
		public FieldType Type { get; set; }
		public string Label { get; set; }
		public string Data { get; set; }
	}
}
