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
    public abstract class PopulationTest
    {
        protected void populationTest(IPopulation population, int food)
        {
            double startFitness = population.Fitness;

            population.Feed(food);

            checkPopulation(population, food, startFitness);
        }

        protected void timedPopulationTest(IPopulation population, int food)
        {
            double startFitness = population.Fitness;

            population.Feed(food);

            DateTime start = DateTime.Now;
            population.Feed(food);
            DateTime endFirstRun = DateTime.Now;
            population.Feed(2 * food);
            DateTime endSecondRun = DateTime.Now;

            checkPopulation(population, 4 * food, startFitness);

            AssertEx.IsLessThanOrEqualTo((endFirstRun - start).TotalMilliseconds, 0.7 * (endSecondRun - endFirstRun).TotalMilliseconds);
            AssertEx.IsGreaterThanOrEqualTo((endFirstRun - start).TotalMilliseconds, 0.3 * (endSecondRun - endFirstRun).TotalMilliseconds);
        }

        protected void evaluatedPopulationTest(IPopulation population, int food)
        {
            double startFitness = population.Fitness;

            AssertEx.IsGreaterThanOrEqualTo(population.Fitness, -50);

            population.Feed(food);

            checkPopulation(population, food, startFitness);

            AssertEx.IsGreaterThanOrEqualTo(population.Fitness, -5);
        }

        protected static void checkPopulation(IPopulation population, int food, double startFitness)
        {
            AssertEx.IsGreaterThanOrEqualTo(population.Fitness, startFitness);
            AssertEx.IsGreaterThanOrEqualTo(population.IndividualMutations, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.IndividualCrossovers, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.IndividualFitnessEvaluations, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.Generations, 1);
            Assert.AreEqual(food, population.FoodConsumedInLifetime);

            Assert.AreEqual(population.Fitness, population.Best.Fitness);

            foreach (IEvolvable evolvable in population.Individuals)
                Assert.IsNotNull(evolvable);                        
        }

        [TestMethod]
        public void BasicTest()
        {
            Random random = new Random(2014);
            populationTest(createTestPopulation(random), reasonableFood());
        }

        [TestMethod]
        public void TimingTest()
        {
            Random random = new Random(2014);
            timedPopulationTest(createTestPopulation(random), reasonableFood() / 4);
        }

        [TestMethod]
        public void FitnessProgressTest()
        {
            Random random = new Random(2014);
            evaluatedPopulationTest(createTestPopulation(random), reasonableFood());
        }

        [TestMethod]
        public void OrderedIndividualsTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            //populationTest(population, reasonableFood());
            population.Feed(reasonableFood());

            IEvolvable latest = population.Individuals.First();
            foreach (IEvolvable next in population.Individuals)
            {
                AssertEx.IsLessThanOrEqualTo(next.Fitness, latest.Fitness);
                latest = next;
            }
        }

        [TestMethod]
        public virtual void TypeOfIndividualsTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            populationTest(population, reasonableFood());

            Assert.IsInstanceOfType(population.Best, typeof(TestEvolvable));
            Assert.IsInstanceOfType(population.Worst, typeof(TestEvolvable));
        }

        protected abstract IPopulation createTestPopulation(Random random);
        protected abstract int reasonableFood();
    }
}
