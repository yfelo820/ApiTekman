using System.Collections.Generic;
using Api.Entities.Content;

/*
	The sole purpose of this class is a fix to the lack of polymorphism support in .NET core.
	When asp's model binder parses a List<Item> made of ItemDrag, ItemDrop, etc, they all are parsed
	as Item and lose their own properties and the type.

	To avoid it, the idea of this is simple. Instead of POSTing a List<Item> we will post a List for
	each type of item, and then concat each of those List in a single List<Item> in which the types 
	will be preserved.
*/
namespace Api.DTO.Backoffice
{
	public class PostSceneDTO<T> where T: Scene {

		public T Scene { get; set; }
		public List<ItemInput> ItemsInput { get; set; }
		public List<ItemSelect> ItemsSelect { get; set; }
		public List<ItemStatic> ItemsStatic { get; set; }
		public List<ItemDrag> ItemsDrag { get; set; }
		public List<ItemDrop> ItemsDrop { get; set; }
		public List<ItemDraw> ItemsDraw { get; set; }
		public List<ItemSelectGroup> ItemsSelectGroup { get; set; }

		public PostSceneDTO()
		{
			ItemsInput = new List<ItemInput>();
			ItemsSelect = new List<ItemSelect>();
			ItemsStatic = new List<ItemStatic>();
			ItemsDrag = new List<ItemDrag>();
			ItemsDrop = new List<ItemDrop>();
			ItemsDraw = new List<ItemDraw>();
			ItemsSelectGroup = new List<ItemSelectGroup>();
		}

		public T GetSceneWithItems() {
			Scene.Items = new List<Item>();
			Scene.Items.AddRange(ItemsInput);
			Scene.Items.AddRange(ItemsSelect);
			Scene.Items.AddRange(ItemsStatic);
			Scene.Items.AddRange(ItemsDrag);
			Scene.Items.AddRange(ItemsDrop);
			Scene.Items.AddRange(ItemsDraw);
			Scene.Items.AddRange(ItemsSelectGroup);
			return Scene;
		}
	}
}
