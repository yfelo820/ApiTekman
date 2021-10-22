using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTO.Teachers
{
    public class TrackingDTOExtended
    {
        public string Name { get; set; }
        public string Lastname1 { get; set; }
        public string Lastname2 { get; set; }
        public string UserName { get; set; }
        public int Session { get; set; }
        public List<ActivityScore> AverageScores { get; set; }

    }

    
}
