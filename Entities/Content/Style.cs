using System;
using System.ComponentModel.DataAnnotations;


namespace Api.Entities.Content
{
	public class Style : BaseEntity
	{
		public Guid ItemId { get; set; }
		
		public string BorderColor { get; set; }
		public int BorderWidth { get; set; }
		public int BorderRadius { get; set; }
		public BorderStyle BorderStyle { get; set; }

		public int ShadowVerticalOffset { get; set; }
		public int ShadowHorizontalOffset { get; set; }
		public int ShadowBlur { get; set; }
		public string ShadowColor { get; set; }
		public float ShadowOpacity { get; set; }
		public bool IsShadowInset { get; set; }

		public string TextShadow { get; set; }

		public string BackgroundColor { get; set; }
	}

	public enum BorderStyle {
		Normal,
		Dashed,
		Dotted
	}
}
