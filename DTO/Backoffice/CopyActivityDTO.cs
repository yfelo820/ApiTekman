using System;
using System.Collections.Generic;

namespace Api.DTO.Backoffice
{
	public class CopyActivityDTO {
		public Guid ActivityId { get; set; }
		public Guid? ContentBlockId { get; set; }
	}
}