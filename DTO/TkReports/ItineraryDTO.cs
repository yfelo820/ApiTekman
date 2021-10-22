using System;
using System.Collections.Generic;

namespace Api.DTO.TkReports
{
	public class ItineraryDTO
	{
		public int Course { get; set; }
    	public int SessionCount { get; set; }
		public IEnumerable<ItinerarySessionDTO> Sessions { get; set; }
	}

	public class ItinerarySessionDTO 
	{
		public int Number { get; set; }
		public IEnumerable<ItineraryContentBlockDTO> ContentBlocks { get; set; }
	}

	public class ItineraryContentBlockDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Order { get; set; }
	}
}