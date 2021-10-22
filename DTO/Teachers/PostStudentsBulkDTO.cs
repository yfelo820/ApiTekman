using System.Collections.Generic;

namespace Api.DTO.Teachers
{
	public class PostStudentsBulkDTO
	{
		public List<string> Errors { get; set; }
        public List<string> InactiveStudentsUpdated { get; set; }
	}
}