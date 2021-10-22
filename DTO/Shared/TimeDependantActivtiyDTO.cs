using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Shared
{
	public class TimeDependantActivityDTO
	{
		public bool IsTimeDependant { get; set; }
		public int WordCount { get; set; }
		public int QuestionCount { get; set; }
		public int Course { get; set; }
	}
}