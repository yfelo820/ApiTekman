using System;
using System.Collections.Generic;
using System.Linq;
using Api.Entities.Content;
using Api.Entities.Schools;

namespace Api.DTO.Students
{
	public class ExerciseDTO {
		
		public string BackgroundImage { get; set; }
		public Transition Transition { get; set; }
		public List<Item> Items  { get; set; }

		public ExerciseDTO(Exercise exercise)
		{
			BackgroundImage = exercise.BackgroundImage;
			Transition = exercise.Transition;
			Items = exercise.Items;
		}
	}
}