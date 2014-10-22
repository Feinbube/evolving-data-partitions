using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestHelpers;

namespace UnitTests
{
    [TestClass]
    public class HierarchicalPopulationTest : PopulationTest
    {
        protected override IPopulation createTestPopulation(Random random)
        {
            return new SelectMutateCrossoverPopulation(null, random,
                new RoundRobinCreator(new List<ICreator>() {
                   new SelectMutateCrossoverPopulationCreator(null, random, new SelectMutateCrossoverPopulationCreator(null, random, new TestCreator(random), 2, 3), 2, 3),
                   new SelectMutateCrossoverPopulationCreator(null, random, new IndividualMutateAndCrossoverPopulationCreator(null, random, new TestCreator(random), 2), 2, 3),
                   new IndividualMutateAndCrossoverPopulationCreator(null, random, new SelectMutateCrossoverPopulationCreator(null, random, new TestCreator(random), 2, 3), 2),
                   new IndividualMutateAndCrossoverPopulationCreator(null, random, new IndividualMutateAndCrossoverPopulationCreator(null, random, new TestCreator(random), 2), 2)
                }), 4) { EnoughFeedingsForBreeding = 20000 };
        }

        protected override int reasonableFood()
        {
            return 13337;
        }
    }
}