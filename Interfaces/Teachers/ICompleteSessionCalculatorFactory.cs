using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Factories;

namespace Api.Interfaces.Teachers
{
    interface ICompleteSessionCalculatorFactory
    {
        CompleteSesssionCalculator Create(string subject);
    }
}
