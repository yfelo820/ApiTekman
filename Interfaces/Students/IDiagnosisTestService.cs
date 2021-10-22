using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Students;
using Api.Entities.Content;

namespace Api.Interfaces.Students
{
	public interface IDiagnosisTestService
	{
		Task<List<Guid>> Get();
		Task Post(double grade);
	}
}