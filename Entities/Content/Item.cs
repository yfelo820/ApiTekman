using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
    public abstract class Item : BaseEntity
    {
        public Guid SceneId { get; set; }

        public string Name { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Rotation { get; set; }
        public int ZIndex { get; set; }

        public string Text { get; set; }
        public string MediaUrl { get; set; }
        public MediaType MediaType { get; set; }
        public AudioPlayerType AudioPlayerType { get; set; }

		public virtual Style Style { get; set; }

		public abstract ItemType Type { get; }
	}

	public enum ItemType {
		Static,
		Drag,
		Drop,
		Input,
		Select,
		Draw,
		SelectGroup
	}

    public enum AudioPlayerType: byte
    {
        Button,
        Controls,
        Answer
    }
}
