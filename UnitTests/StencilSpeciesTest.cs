using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFieldLayoutSimulation;
using EvolutionFramework;

namespace UnitTests
{
    [TestClass]
    public class StencilSpeciesTest
    {
        [TestMethod]
        public void CreationTest()
        {
            Random random = new Random(2014);

            var generator = new StencilSpeciesCreator(random, 10, 10, new double[] { 0.09, 0.16, 0.75 });
            for (int i = 0; i < 10000; i++)
                Assert.IsTrue((generator.Create() as StencilSpecies).IsValid);
        }

        [TestMethod]
        public void MutationTest()
        {
            Random random = new Random(2014);

            var generator = new StencilSpeciesCreator(random, 10, 10, new double[] { 0.09, 0.16, 0.75 });
            for (int i = 0; i < 100; i++)
            {
                var specimen = (StencilSpecies)generator.Create();
                for (int i2 = 0; i2 < 100; i2++)
                {
                    specimen.Mutate(random);
                    Assert.IsTrue(specimen.IsValid);
                }
            }
        }

        [TestMethod]
        public void CrossoverTest()
        {
            Random random = new Random(2014);

            var generator = new StencilSpeciesCreator(random, 10, 10, new double[] { 0.09, 0.16, 0.75 });
            for (int i = 0; i < 1000; i++)
            {
                var specimen1 = (StencilSpecies)generator.Create();
                var specimen2 = (StencilSpecies)generator.Create();
                var child = (StencilSpecies)specimen1.Crossover(random, specimen2);
                Assert.IsTrue(child.IsValid);
            }
        }

        [TestMethod]
        public void CrossoverFillWithEqualGenesTest()
        {
            int[] field = new int[6];
            int[] field1 = new int[] { 0, 0, 1, 2, 2, 2 };
            int[] field2 = new int[] { 0, 2, 2, 1, 0, 2 };
            int[] cellsPerProcessor = new int[] { 2, 1, 3 };
            int count = StencilSpecies.fillWithEqualGenesOrMinusOne(field, field1, field2, cellsPerProcessor);
            Assert.AreEqual(2, count);
            AssertAreEqual(new int[] { 0, -1, -1, -1, -1, 2 }, field);
            AssertAreEqual(new int[] { 1, 1, 2 }, cellsPerProcessor);
        }

        [TestMethod]
        public void CrossoverFreePositionTest()
        {
            Random random = new Random(2014);

            int[] field = new int[] { -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < 6; i++)
            {
                AssertIsGreaterThanOrEqualTo(StencilSpecies.freePosition(random, 6 - i, field), i);
                field[i] = 0;
            }

            field = new int[] { -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < 6; i++)
            {
                int position = StencilSpecies.freePosition(random, 6 - i, field);
                field[position] = 0;
            }
            Assert.AreEqual(0, field.Sum());
        }

        [TestMethod]
        public void OverheadTest()
        {
            Assert.AreEqual(8, StencilSpecies.Overhead(new int[] { 1, 2, 1, 2, 2, 2, 1, 2, 1 }, 3, 3));
            Assert.AreEqual(9, StencilSpecies.Overhead(new int[] { 1, 2, 1, 2, 1, 2, 1, 2, 1 }, 3, 3));
            Assert.AreEqual(15, StencilSpecies.Overhead(new int[] { 1, 2, 1, 2, 2, 1, 2, 1, 1, 2, 2, 2, 2, 1, 2, 1 }, 4, 4));

            Assert.AreEqual(13, StencilSpecies.Overhead(new int[] { 1, 2, 1, 2, 3, 2, 1, 2, 1 }, 3, 3));
            Assert.AreEqual(8, StencilSpecies.Overhead(new int[] { 1, 2, 3, 4 }, 2, 2));

            Assert.AreEqual(24, StencilSpecies.Overhead(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3, 3));
            Assert.AreEqual(11, StencilSpecies.Overhead(new int[] { 1, 1, 2, 2, 1, 1, 3, 3, 3, 3, 3, 3 }, 4, 3));
            Assert.AreEqual(40, StencilSpecies.Overhead(new int[] { 
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                3, 3, 3, 3, 3, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 4, 4, 4, 4, 4
            }, 10, 10));
            Assert.AreEqual(39, StencilSpecies.Overhead(new int[] { 
                1, 1, 1, 1, 1, 1, 1, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 2, 2, 2, 2,
                1, 3, 1, 1, 1, 2, 2, 2, 2, 2,
                3, 3, 3, 1, 4, 4, 2, 2, 2, 2,
                3, 3, 3, 4, 4, 4, 4, 2, 2, 2,
                3, 3, 3, 3, 4, 4, 4, 4, 2, 2,
                3, 3, 3, 3, 3, 4, 4, 4, 4, 2,
                3, 3, 3, 3, 3, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 4, 4, 4, 4, 4, 4
            }, 10, 10));
            Assert.AreEqual(80, StencilSpecies.Overhead(new int[] { 
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            }, 20, 20));
        }

        [TestMethod]
        public void OverheadNewTest()
        {
            Assert.AreEqual(10, StencilSpecies.OverheadNew(new int[] { 25, 25, 25, 25 }, new int[] { 
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 2, 2, 2, 2, 2,
                3, 3, 3, 3, 3, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 0, 0, 0, 0, 0
            }, 10, 10));
            Assert.AreEqual(12, StencilSpecies.OverheadNew(new int[] { 25, 25, 25, 25 }, new int[] { 
                1, 1, 1, 1, 1, 1, 1, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 2, 2, 2, 2,
                1, 3, 1, 1, 1, 2, 2, 2, 2, 2,
                3, 3, 3, 1, 0, 0, 2, 2, 2, 2,
                3, 3, 3, 0, 0, 0, 0, 2, 2, 2,
                3, 3, 3, 3, 0, 0, 0, 0, 2, 2,
                3, 3, 3, 3, 3, 0, 0, 0, 0, 2,
                3, 3, 3, 3, 3, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 0, 0, 0, 0, 0, 0
            }, 10, 10));
            Assert.AreEqual(20, StencilSpecies.OverheadNew(new int[] { 100, 100, 100, 100 }, new int[] { 
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            }, 20, 20));
        }

        [TestMethod]
        public void UniqueNeighborCountTest()
        {
            Assert.AreEqual(0, StencilSpecies.UniqueNeighborCount(new int[] { 1, 2, 1, 2, 2, 2, 1, 2, 1 }, 1, 1, 3, 3));
            Assert.AreEqual(1, StencilSpecies.UniqueNeighborCount(new int[] { 1, 2, 1, 2, 1, 2, 1, 2, 1 }, 1, 1, 3, 3));
        }

        [TestMethod]
        public void NoClonesAroundTest()
        {
            Random random = new Random(2014);
            BreedingPool pool = new BreedingPool(new StencilSpeciesCreator(random, 10, 10, new double[] { 0.09, 0.16, 0.75 }), 25);
            pool.Feed(random, 10000);

            for (int i = 0; i < pool.PopulationSize - 1; i++)
                for (int j = i + 1; j < pool.PopulationSize; j++)
                    if (pool.Population[i].Equals(pool.Population[j]))
                        throw new Exception("Clones!! " + i + " and " + j);
        }

        public static void AssertAreEqual(int[] expected, int[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i], "at index " + i);
        }

        public static void AssertIsGreaterThanOrEqualTo(int actual, int value)
        {
            Assert.IsTrue(actual >= value, actual + " is less than " + value);
        }
    }
}
