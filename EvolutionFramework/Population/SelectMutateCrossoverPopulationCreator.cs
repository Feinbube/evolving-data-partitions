using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class SelectMutateCrossoverPopulationCreator : ICreator
    {
        Random random;
        ICreator creator = null;
        int populationSize = 0;
        int enoughFeedingsForEvolution = 0;
        IPopulation population = null;

        public SelectMutateCrossoverPopulationCreator(IPopulation population, Random random, ICreator creator, int populationSize, int enoughFeedingsForEvolution) { this.population = population; this.random = random; this.creator = creator; this.populationSize = populationSize; this.enoughFeedingsForEvolution = enoughFeedingsForEvolution; }

        public IEvolvable Create() { return new SelectMutateCrossoverPopulation(population, random, creator, populationSize) { EnoughFeedingsForBreeding = enoughFeedingsForEvolution }; }
    }
}
