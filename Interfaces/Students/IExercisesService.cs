using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Students;
using Api.Entities.Content;

namespace Api.Interfaces.Students
{
	public interface IExercisesService
	{
		Task<List<Guid>> GetByActivityId (Guid activityId);
		Task<ExerciseDTO> GetSingle(Guid id);
	}
}