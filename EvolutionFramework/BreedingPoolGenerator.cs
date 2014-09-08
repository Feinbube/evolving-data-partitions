using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class BreedingPoolCreator : ICreator
    {
        ICreator creator = null;
        int population = 0;
        int enoughFeedingsForEvolution = 0;

        public BreedingPoolCreator(ICreator creator, int population, int enoughFeedingsForEvolution) { this.creator = creator; this.population = population; this.enoughFeedingsForEvolution = enoughFeedingsForEvolution; }

        public IEvolvable Create() { return new BreedingPool(creator, population) { EnoughFeedingsForBreeding = enoughFeedingsForEvolution }; }
    }
}
