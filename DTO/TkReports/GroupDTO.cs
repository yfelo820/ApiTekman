using System;
using System.Collections.Generic;

namespace Api.DTO.TkReports
{
	public class GroupDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Course { get; set; }
    	public IEnumerable<StudentNameDTO> Students { get; set; }
	}
}