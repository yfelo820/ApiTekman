using System.Collections.Generic;

namespace Api.Entities.Content
{
	public class ItemDrag : Item
	{
		public bool IsMultiple { get; set; }
		public virtual List<DragDrop> DropAnswers { get; set; }
		public override ItemType Type => ItemType.Drag;
	}
}
