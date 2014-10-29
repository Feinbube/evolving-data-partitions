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
    public class PopulationCrossingTest
    {
        [TestMethod]
        public void CrossingTest()
        {
            Random random = new Random(2014);

            EvolvablePopulation select = new SelectMutateCrossoverPopulation(null, random, new TestCreator(random), 20);
            EvolvablePopulation individual = new IndividualMutateAndCrossoverPopulation(null, random, new TestCreator(random), 20);

            select.Feed(100);
            individual.Feed(100);

            EvolvablePopulation child1 = (EvolvablePopulation)select.Crossover(individual);
            Assert.IsInstanceOfType(child1, typeof(SelectMutateCrossoverPopulation));
            Assert.AreEqual(select.Fitness, child1.Fitness);

            Assert.AreEqual(40, child1.PopulationSize);
            child1.Feed(100);
            Assert.AreEqual(20, child1.PopulationSize);
            AssertEx.IsGreaterThanOrEqualTo(child1.Fitness, select.Fitness);

            individual.Feed(1000);

            EvolvablePopulation child2 = (EvolvablePopulation)select.Crossover(individual);
            Assert.IsInstanceOfType(child2, typeof(IndividualMutateAndCrossoverPopulation));
            Assert.AreEqual(individual.Fitness, child2.Fitness);
            AssertEx.IsGreaterThanOrEqualTo(child2.Fitness, individual.Fitness);

            Assert.AreEqual(40, child2.PopulationSize);
            child2.Feed(100);
            Assert.AreEqual(20, child2.PopulationSize);
        }
    }
}