using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public interface INotingEvolvable : IEvolvable
    {
        int Mutations { get; }
        int Crossovers { get; }
        int FitnessEvaluations { get; }
    }
}
