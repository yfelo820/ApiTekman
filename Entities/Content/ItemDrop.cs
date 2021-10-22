using System.Collections.Generic;

namespace Api.Entities.Content
{
	public class ItemDrop : Item
	{
		public virtual List<DragDrop> DragAnswers { get; set; }
		public override ItemType Type => ItemType.Drop;
	}
}
