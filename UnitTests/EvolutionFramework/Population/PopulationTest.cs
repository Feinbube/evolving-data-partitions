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
        [TestMethod]
        public virtual void BasicTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            int food = reasonableFood();
            double startFitness = population.Fitness;

            population.Feed(food);

            AssertEx.IsGreaterThanOrEqualTo(population.Fitness, startFitness);
            Assert.AreEqual(food, population.FoodConsumedInLifetime);

            basicTest(population);
        }

        protected virtual void basicTest(IPopulation population)
        {
            Assert.AreEqual(population.Fitness, population.Best.Fitness);

            AssertEx.IsGreaterThanOrEqualTo(population.Individuals.Count, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.FoodConsumedInLifetime, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.FitnessEvaluations, 1);

            AssertEx.IsGreaterThanOrEqualTo(population.Generations, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.Mutations, 1);
            AssertEx.IsGreaterThanOrEqualTo(population.Crossovers, 1);

            Assert.IsNotInstanceOfType(population.BestOfAllTime, typeof(IPopulation));
            Assert.IsNotInstanceOfType(population.BestOfAllTime, typeof(IEvolver));

            if(population is EvolvablePopulation)
                AssertEx.IsGreaterThanOrEqualTo((population as EvolvablePopulation).FullPopulation, population.Individuals.Count);

            foreach (IEvolvable evolvable in population.Individuals)
                Assert.IsNotNull(evolvable);

            foreach (var individual in population.Individuals)
                if (individual is IPopulation)
                    basicTest(individual as IPopulation);
        }

        [TestMethod]
        public void TimingTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            int food = reasonableFood() / 5;
            double startFitness = population.Fitness;

            population.Feed(food);

            DateTime start = DateTime.Now;
            population.Feed(food);
            DateTime endFirstRun = DateTime.Now;
            population.Feed(4 * food);
            DateTime endSecondRun = DateTime.Now;

            // t(4x) <= t(x) * 4 + 50%
            AssertEx.IsLessThanOrEqualTo((endSecondRun - endFirstRun).TotalMilliseconds, (endFirstRun - start).TotalMilliseconds * 4.0 * 1.5);
        }

        [TestMethod]
        public void NoFitnessDegredationTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            int food = reasonableFood() / 10;
            double startFitness = population.Fitness;

            double fitness = population.Fitness;
            for (int i = 0; i < 20; i++)
            {
                population.Feed(food);
                AssertEx.IsGreaterThanOrEqualTo(population.Fitness, fitness);
                fitness = population.Fitness;
            }
        }

        [TestMethod]
        public void ReasonableFitnessProgressionTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            int food = reasonableFood();
            double startFitness = population.Fitness;

            AssertEx.IsGreaterThanOrEqualTo(population.Fitness, -50);

            population.Feed(food);

            AssertEx.IsGreaterThanOrEqualTo(population.Fitness, -5);
        }
       
        [TestMethod]
        public void OrderedIndividualsTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            
            population.Feed(reasonableFood());

            orderedIndividualsTest(population);
        }

        protected static void orderedIndividualsTest(IPopulation population)
        {
            IEvolvable latest = population.Best;
            foreach (IEvolvable next in population.IndividualsSortedByFitness)
            {
                AssertEx.IsLessThanOrEqualTo(next.Fitness, latest.Fitness);
                latest = next;
            }

            foreach (IEvolvable evolvable in population.Individuals)
                Assert.IsNotNull(evolvable);

            foreach (var individual in population.Individuals)
                if (individual is IPopulation)
                    orderedIndividualsTest(individual as IPopulation);
        }

        [TestMethod]
        public virtual void TypeOfIndividualsTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            population.Feed(reasonableFood() / 5);

            typeOfIndividualsTest(population);
        }

        protected static void typeOfIndividualsTest(IPopulation population)
        {
            if (population.Best is IPopulation)
            {
                Assert.IsInstanceOfType(population.Best, typeof(EvolvablePopulation));
                Assert.IsInstanceOfType(population.Worst, typeof(EvolvablePopulation));

                typeOfIndividualsTest(population.Best as IPopulation);
                typeOfIndividualsTest(population.Worst as IPopulation);
            }
            else
            {
                Assert.IsInstanceOfType(population.Best, typeof(TestEvolvable));
                Assert.IsInstanceOfType(population.Worst, typeof(TestEvolvable));
            }
        }

        [TestMethod]
        public void NoClonesAroundTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            int food = reasonableFood() / 10;

            for (int g = 0; g < 10; g++)
            {
                population.Feed(food);
                noClonesAroundTest(population);
            }
        }

        protected void noClonesAroundTest(IPopulation population)
        {
            foreach (IEvolvable evolvable in population.Individuals)
                Assert.IsNotNull(evolvable);

            for (int i = 0; i < population.Individuals.Count - 1; i++)
                for (int j = i + 1; j < population.Individuals.Count; j++)
                    if (population.Individuals[i].Equals(population.Individuals[j]))
                        throw new Exception("Clones!! " + i + " and " + j);

            foreach (var individual in population.Individuals)
                if (individual is IPopulation)
                    noClonesAroundTest(individual as IPopulation);
        }

        [TestMethod]
        public void CloneTest()
        {
            Random random = new Random(2014);
            IPopulation population = createTestPopulation(random);
            if(population is EvolvablePopulation)
            {
                population.Feed(reasonableFood());

                EvolvablePopulation original =  (EvolvablePopulation)population;
                EvolvablePopulation clone = (EvolvablePopulation)original.Clone();

                Assert.AreEqual(original.EvolveMutations, clone.EvolveMutations );
                Assert.AreEqual(original.EvolveCrossovers, clone.EvolveCrossovers);
                Assert.AreEqual(original.EvolveFitnessEvaluations, clone.EvolveFitnessEvaluations);
            }
        }

        protected abstract IPopulation createTestPopulation(Random random);
        protected abstract int reasonableFood();
    }
}
