using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public interface IEvolvable
    {
        IEvolvable Clone();
        void Mutate(Random random);
        IEvolvable Crossover(Random random, IEvolvable other);
        double Fitness { get; }
        void Feed(Random random, int resources);
    }
}
