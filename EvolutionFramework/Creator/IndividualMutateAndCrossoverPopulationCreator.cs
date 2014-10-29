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
        int populationSize = 0;
        IPopulation population = null;

        public IndividualMutateAndCrossoverPopulationCreator(IPopulation population, Random random, ICreator creator, int populationSize) { this.population = population;  this.random = random; this.creator = creator; this.populationSize = populationSize; }

        public IEvolvable Create() { return new IndividualMutateAndCrossoverPopulation(population, random, creator, populationSize) { }; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
