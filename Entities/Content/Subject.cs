namespace Api.Entities.Content
{
	public class Subject : BaseEntity
	{
		public string Name { get; set; }
		public int SessionCount { get; set; }
		public int DifficultyCount { get; set; }
		public string Key { get; set; }
	}
}