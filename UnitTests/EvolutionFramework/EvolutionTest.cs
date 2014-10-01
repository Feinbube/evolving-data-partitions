using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestHelpers;
using DataFieldLayoutSimulation;

namespace UnitTests
{
    [TestClass]
    public class EvolutionTest : PopulationTest
    {
        protected override IPopulation createTestPopulation(Random random)
        {
            return new Evolution(random, new TestCreator(random), 7, 64) { EnoughFeedingsForBreeding = 1000 };
        }

        protected override int reasonableFood()
        {
            return 20000;
        }

        [TestMethod]
        public override void TypeOfIndividualsTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            populationTest(population, reasonableFood());

            Assert.IsInstanceOfType(population.Best, typeof(EvolvablePopulation));
            Assert.IsInstanceOfType(population.Worst, typeof(EvolvablePopulation));

            Assert.IsInstanceOfType((population.Best as EvolvablePopulation).Best, typeof(TestEvolvable));
            Assert.IsInstanceOfType((population.Worst as EvolvablePopulation).Worst, typeof(TestEvolvable));
        }
    }
}