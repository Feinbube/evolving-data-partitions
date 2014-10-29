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
            /*
            return new SelectMutateCrossoverPopulation(null, random,
                new RoundRobinCreator(new List<ICreator>() {
                   new SelectMutateCrossoverPopulationCreator(null, random, new SelectMutateCrossoverPopulationCreator(null, random, new TestCreator(random), 7, 31), 4, 27),
                   new SelectMutateCrossoverPopulationCreator(null, random, new IndividualMutateAndCrossoverPopulationCreator(null, random, new TestCreator(random), 5), 4, 27),
                   new IndividualMutateAndCrossoverPopulationCreator(null, random, new SelectMutateCrossoverPopulationCreator(null, random, new TestCreator(random), 7, 31), 5),
                   new IndividualMutateAndCrossoverPopulationCreator(null, random, new IndividualMutateAndCrossoverPopulationCreator(null, random, new TestCreator(random), 5), 5)
                }), 4) { EnoughFeedingsForBreeding = 20000 }; */

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

        [TestMethod]
        public virtual void TypeOfChildrenTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            population.Feed(reasonableFood());

            Assert.IsInstanceOfType(population.Individuals[0], typeof(SelectMutateCrossoverPopulation));
            Assert.IsInstanceOfType((population.Individuals[0] as SelectMutateCrossoverPopulation).Individuals[0], typeof(SelectMutateCrossoverPopulation));

            Assert.IsInstanceOfType(population.Individuals[1], typeof(SelectMutateCrossoverPopulation));
            Assert.IsInstanceOfType((population.Individuals[1] as SelectMutateCrossoverPopulation).Individuals[0], typeof(IndividualMutateAndCrossoverPopulation));

            Assert.IsInstanceOfType(population.Individuals[2], typeof(IndividualMutateAndCrossoverPopulation));
            Assert.IsInstanceOfType((population.Individuals[2] as IndividualMutateAndCrossoverPopulation).Individuals[0], typeof(SelectMutateCrossoverPopulation));

            Assert.IsInstanceOfType(population.Individuals[3], typeof(IndividualMutateAndCrossoverPopulation));
            Assert.IsInstanceOfType((population.Individuals[3] as IndividualMutateAndCrossoverPopulation).Individuals[0], typeof(IndividualMutateAndCrossoverPopulation));
        }

        protected override void basicTest(IPopulation population)
        {
            Assert.AreEqual(population.Fitness, population.Best.Fitness);

            AssertEx.IsGreaterThanOrEqualTo(population.Individuals.Count, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.FoodConsumedInLifetime, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.FitnessEvaluations, 1);

            Assert.IsNotInstanceOfType(population.BestOfAllTime, typeof(IPopulation));
            Assert.IsNotInstanceOfType(population.BestOfAllTime, typeof(IEvolver));

            if (population is EvolvablePopulation)
                AssertEx.IsGreaterThanOrEqualTo((population as EvolvablePopulation).FullPopulation, population.Individuals.Count);

            foreach (IEvolvable evolvable in population.Individuals)
                Assert.IsNotNull(evolvable);

            foreach (var individual in population.Individuals)
                if (individual is IPopulation)
                    basicTest(individual as IPopulation);
        }
    }
}