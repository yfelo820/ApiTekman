using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Schools;
using Api.Interfaces.Teachers;

namespace Api.Factories
{
    public class CompleteSessionEmatCalculator : CompleteSesssionCalculator
    {
        public override bool CompletedSession(List<StudentAnswer> studentAnwsers)
        {
            if (studentAnwsers.All(b => b.Grade >= Config.PassGrade) && studentAnwsers.Count == Config.StageCountInEmatSession) 
                return true;
            return false;
        }

        public override int MaxDifficulty()
        {
            return Config.MaxDifficultyEmat;
        }

        public override int StagesForForwardSuccessfuly()
        {
            return Config.StageCountInEmatSession;
        }
    }
}
