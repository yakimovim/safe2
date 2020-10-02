namespace Safe.Importer.Model
{
    public sealed class TopicDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public TopicDto[] SubTopics { get; set; }
		public FieldDto[] Fields { get; set; }
	}
}
