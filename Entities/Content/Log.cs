using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
	public class Log : BaseEntity
	{
		public string Text { get; set; }

		public Log(string text)
		{
			Text = text;
		}
	}
}