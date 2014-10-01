using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public interface IEvolvable
    {
        double Fitness { get; }

        void Mutate();
        
        IEvolvable Crossover(IEvolvable other);

        IEvolvable Clone();

        double DifferenceTo(IEvolvable other);
    }
}
