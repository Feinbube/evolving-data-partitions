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
                   new SelectMutateCrossoverPopulation(null, random, new SelectMutateCrossoverPopulationCreator(null, random, new TestCreator(random), 3, 3), 2),
                   new SelectMutateCrossoverPopulation(null, random, new IndividualMutateAndCrossoverPopulationCreator(null, random, new TestCreator(random), 3), 2),
                   new IndividualMutateAndCrossoverPopulationCreator(null, random, new SelectMutateCrossoverPopulationCreator(null, random, new TestCreator(random), 3, 3), 2),
                   new IndividualMutateAndCrossoverPopulationCreator(null, random, new IndividualMutateAndCrossoverPopulationCreator(null, random, new TestCreator(random), 3), 2)
                }), 4);
        }

        protected override int reasonableFood()
        {
            return 2000;
        }

        [TestMethod]
        public override void TypeOfIndividualsTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            population.Feed(reasonableFood()/10);

            Assert.IsInstanceOfType(population.Best, typeof(EvolvablePopulation));
            Assert.IsInstanceOfType(population.Worst, typeof(EvolvablePopulation));

            Assert.IsInstanceOfType((population.Best as EvolvablePopulation).Best, typeof(EvolvablePopulation));
            Assert.IsInstanceOfType((population.Worst as EvolvablePopulation).Worst, typeof(EvolvablePopulation));

            Assert.IsInstanceOfType(((population.Best as EvolvablePopulation).Best as EvolvablePopulation).Best, typeof(TestEvolvable));
            Assert.IsInstanceOfType(((population.Worst as EvolvablePopulation).Worst as EvolvablePopulation).Worst, typeof(TestEvolvable));
        }
    }
}