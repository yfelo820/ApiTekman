using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Teachers;

namespace Api.Interfaces.Teachers
{
	public interface ITimeDependantSessionsService
	{
		Task<List<SessionGroupDTO>> GetSessionsBlocked(Guid groupId);
		Task UpdateSessionsBlocked(Guid groupId, SessionGroupDTO sessionGroupDTO);
	}
}