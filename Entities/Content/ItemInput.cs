namespace Api.Entities.Content
{
	public class ItemInput : Item
	{
		public string Solution { get; set; }
		public bool IsNumber { get; set; }
		public bool IsIgnoringCaps { get; set; }
		public bool IsIgnoringAccents { get; set; }
		public override ItemType Type => ItemType.Input;
	}
}
