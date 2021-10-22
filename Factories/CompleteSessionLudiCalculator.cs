using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Schools;
using Api.Interfaces.Teachers;

namespace Api.Factories
{
    public class CompleteSessionLudiCalculator : CompleteSesssionCalculator 
    {
        public override bool CompletedSession(List<StudentAnswer> studentAnwsers)
        {
            if (studentAnwsers.All(b => b.Grade >= 0) && studentAnwsers.Count == Config.StageCountInLudiSession) 
                return true;
            return false;
        }

        public override int MaxDifficulty()
        {
            return Config.MaxDifficultyLudi;
        }

        public override int StagesForForwardSuccessfuly()
        {
            return Config.StageCountInLudiSession;
        }
    }
}
