
using System;

namespace Api.DTO.Students
{
	public class StageActivityDTO {
		public Guid ActivityId { get; set; }
		public float Grade { get; set; }
		public string ContentBlockName { get; set; }
		public string Image { get; set; }
		public int Stage { get; set; }
	}
}