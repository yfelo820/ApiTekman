using System;

namespace Api.DTO.Backoffice.ProblemResolution
{
    public class ProblemResolutionActivitiesDTO
    {
        public Guid Id { get; set; }

        public int Course { get; set; }

        public int Session { get; set; }

        public int Stage { get; set; }

        public int State { get; set; }
    }
}
