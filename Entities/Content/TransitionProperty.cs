using System;
using System.ComponentModel.DataAnnotations;


namespace Api.Entities.Content
{
	public class TransitionProperty : BaseEntity
	{
		[Required]
		public Guid TransitionId { get; set; }

		public ItemType ItemType { get; set; }
		public ItemState ItemState  { get; set; }
		public PropertyType Property { get; set; }
		public string Value { get; set; }
	}

	public enum ItemState {
		Normal,
		Hover,
		// For select items: when they have been selected
		Selected,
		// For drag items: being dragged
		Dragging,
		// For drag items: being dragged over a drop item
		// For drop items: having a drag item being dragged over them
		Dragover,
		// For drag items: being dropped on a drop item
		// For drop items: having one or more drag items dropped over them
		Dropped,
		// For input items: when the input is focused
		Focus
	}

	public enum PropertyType {
		Blur,
		Grayscale,
		Scale,
		Opacity,
		Shadow,
		Brightness,
		Background
	}
}