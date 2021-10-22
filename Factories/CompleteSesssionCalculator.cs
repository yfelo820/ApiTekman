using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;

namespace Api.Factories
{
    public abstract class CompleteSesssionCalculator
    {         
        public abstract bool CompletedSession(List<StudentAnswer> studentAnwsers);
        public abstract int StagesForForwardSuccessfuly();
        public abstract int MaxDifficulty();
    }
}
