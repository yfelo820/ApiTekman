using System;

namespace Api.Entities.Content
{
	public class ItemSelect : Item
	{
		public bool IsCorrect { get; set; }
		public override ItemType Type => ItemType.Select;
		public Guid? BelongsGroupSelect { get; set; }
	}
}
