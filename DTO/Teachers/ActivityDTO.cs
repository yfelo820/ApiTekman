using System;
using System.Collections.Generic;
using System.Linq;
using Api.Entities.Content;
using Api.Entities.Schools;

namespace Api.DTO.Teachers
{
	public class ActivityDTO {

		public Guid Id { get; set; }
		public int Course { get; set; }		
		public int Session { get; set; }
		public int Stage { get; set; }
		public int Difficulty { get; set; }
		public string Description { get; set; }
		
		public ActivityDTO(Activity activity)
		{
			Id = activity.Id;
			Course = activity.Course.Number;
			Session = activity.Session;
			Stage = activity.Stage;
			Difficulty = activity.Difficulty;
			Description = activity.Description;
		}
	}
}