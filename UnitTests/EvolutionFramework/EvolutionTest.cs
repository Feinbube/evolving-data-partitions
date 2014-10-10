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
    public class EvolutionTest : HierarchicalPopulationTest
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
        public void FitnessProgressionTest()
        {
            Random random = new Random(2014);
            var population = createTestPopulation(random);
            double food = reasonableFood() / 2;

            double fitness = population.Fitness;
            for (int i = 0; i < 20; i++)
            {
                population.Feed(food);
                AssertEx.IsGreaterThanOrEqualTo(population.Fitness, fitness);
                fitness = population.Fitness;
            }
        }
    }
}