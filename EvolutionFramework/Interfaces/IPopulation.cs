using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public interface IPopulation : IFeedable, INotifyPropertyChanged
    {
        long Generations { get; }

        double Fitness { get; }

        IEvolvable BestOfAllTime { get; }
        IEvolvable Best { get; }
        IEvolvable Worst { get; }

        List<IEvolvable> Individuals { get; }
        List<IEvolvable> IndividualsSortedByFitness { get; }

        long Mutations { get; }
        long Crossovers { get; }
        long FitnessEvaluations { get; }

        void NoteMutation();
        void NoteCrossovers();
        void NoteFitnessEvaluations();
    }
}
