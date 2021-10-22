using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
	public class DragDrop : BaseEntity
	{
		public Guid ItemDragId { get; set; }
		public Guid ItemDropId { get; set; }
		public int MultipleDragResult { get; set; }
	}
}