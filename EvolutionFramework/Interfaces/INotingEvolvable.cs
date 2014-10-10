using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public interface INotingEvolvable : IEvolvable
    {
        long Mutations { get; }
        long Crossovers { get; }
        long FitnessEvaluations { get; }
    }
}
