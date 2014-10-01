using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class IndividualMutateAndCrossoverPopulationCreator : ICreator
    {
        Random random;
        ICreator creator = null;
        int population = 0;

        public IndividualMutateAndCrossoverPopulationCreator(Random random, ICreator creator, int population) { this.random = random; this.creator = creator; this.population = population; }

        public IEvolvable Create() { return new IndividualMutateAndCrossoverPopulation(random, creator, population) { }; }
    }
}
