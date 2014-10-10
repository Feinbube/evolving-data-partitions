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
            Assert.AreEqual(food, population.FoodConsumedInLifetime);

            checkPopulation(population);
        }

        protected static void checkPopulation(IPopulation population)
        {
            AssertEx.IsGreaterThanOrEqualTo(population.Generations, 1);

            checkMutations(population);
            checkCrossovers(population);
            checkFitnessEvaluations(population);

            Assert.AreEqual(population.Fitness, population.Best.Fitness);

            foreach (IEvolvable evolvable in population.Individuals)
                Assert.IsNotNull(evolvable);

            foreach (var individual in population.Individuals)
                if (individual is IPopulation)
                    checkPopulation(individual as IPopulation);
        }

        private static void checkMutations(IPopulation population)
        {
            long all = population.Mutations;
            AssertEx.IsGreaterThanOrEqualTo(all, 1);

            bool checkAgain = false;
            foreach (var individual in population.Individuals)
            {
                if (individual is INotingEvolvable)
                {
                    checkAgain = true;
                    all -= (individual as INotingEvolvable).Mutations;
                }
            }

            if (checkAgain)
                Assert.AreEqual(0, all);
        }

        private static void checkCrossovers(IPopulation population)
        {
            long all = population.Crossovers;
            AssertEx.IsGreaterThanOrEqualTo(all, 1);

            bool checkAgain = false;
            foreach (var individual in population.Individuals)
            {
                if (individual is INotingEvolvable)
                {
                    checkAgain = true;
                    all -= (individual as INotingEvolvable).Crossovers;
                }
            }

            if (checkAgain)
                Assert.AreEqual(0, all);
        }

        private static void checkFitnessEvaluations(IPopulation population)
        {
            long all = population.FitnessEvaluations;
            AssertEx.IsGreaterThanOrEqualTo(all, 1);

            bool checkAgain = false;
            foreach (var individual in population.Individuals)
            {
                if (individual is INotingEvolvable)
                {
                    checkAgain = true;
                    all -= (individual as INotingEvolvable).FitnessEvaluations;
                }
            }

            if (checkAgain)
                Assert.AreEqual(0, all);
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

            IEvolvable latest = population.IndividualsSortedByFitness.First();
            foreach (IEvolvable next in population.IndividualsSortedByFitness)
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
