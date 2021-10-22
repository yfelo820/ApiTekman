using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Students;
using Api.Entities.Content;

namespace Api.Interfaces.Students
{
	public interface IFeedbacksService
	{
		Task<Feedback> Get (Guid activityId, float mark); 
	}
}