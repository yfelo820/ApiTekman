using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Teachers;

namespace Api.Interfaces.Teachers
{
	public interface IStudentsService
	{
		Task<List<StudentDTO>> GetAll();
		Task<StudentDTO> GetSingle(string userName);
        Task<List<StudentDTO>> GetMultiples(List<string> userNames);
    }
}