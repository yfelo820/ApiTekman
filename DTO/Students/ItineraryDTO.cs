using System.Collections.Generic;
using Api.Entities.Schools;

namespace Api.DTO.Students
{
    public class ItineraryDTO
    {
        public List<ItinerarySessionDTO> Sessions { get; set; }
        public bool HasDiagnosisTest { get; set; }
        public int StudentProgressCourse { get; set; }
        public int StudentProgressSession { get; set; }
    }

    public class ItinerarySessionDTO
    {
        public int Key { get; set; }
        public int Session { get; set; }
        public int Course { get; set; }
        public SessionState State { get; set; }
    }

    public enum SessionState
    {
        Completed,
        Current,
        Pending,
        Unreachable,
        Locked
    }
}