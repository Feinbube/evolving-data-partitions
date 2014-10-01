using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFieldLayoutSimulation;
using EvolutionFramework;
using UnitTestHelpers;
using ExecutionEnvironment;

namespace UnitTests
{
    [TestClass]
    public class StencilSpeciesTest
    {
        Random random = new Random(2014);

        private StencilSpeciesArr create()
        {
            StencilSpeciesArr result = new StencilSpeciesArr(random, 10, 10, new int[] { 9, 16, 75 });
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.IsValid);
            return result;
        }

        private StencilSpeciesArr create(int[] cellsPerProcessor, int[] field, int w, int h)
        {
            StencilSpeciesArr result = new StencilSpeciesArr(random, new Arr<int>(field, w, h), cellsPerProcessor, new Mutator[] { new JumpToSameColorMutator() });
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.IsValid);
            return result;
        }

        [TestMethod]
        public void CreationTest()
        {
            for (int i = 0; i < 1000; i++)
                create();
        }

        [TestMethod]
        public void MutationTest()
        {
            Random random = new Random(2014);

            for (int i = 0; i < 100; i++)
            {
                var specimen = create();
                for (int i2 = 0; i2 < 100; i2++)
                {
                    specimen.Mutate();
                    Assert.IsTrue(specimen.IsValid);
                }
            }
        }

        [TestMethod]
        public void CrossoverTest()
        {
            Random random = new Random(2014);

            for (int i = 0; i < 1000; i++)
            {
                var specimen1 = create();
                var specimen2 = create();
                var child = (StencilSpeciesArr)specimen1.Crossover(specimen2);
                Assert.IsTrue(child.IsValid);
            }
        }

        [TestMethod]
        public void CrossoverFillWithEqualGenesTest()
        {
            Arr<int> field = new Arr<int>(6);
            Arr<int> field1 = new Arr<int>(new int[] { 0, 0, 1, 2, 2, 2 });
            Arr<int> field2 = new Arr<int>(new int[] { 0, 2, 2, 1, 0, 2 });
            int[] cellsPerProcessor = new int[] { 2, 1, 3 };
            int count = StencilSpeciesArr.fillWithEqualGenesOrMinusOne(field, field1, field2, cellsPerProcessor);
            Assert.AreEqual(2, count);
            AssertEx.AreEqual(new int[] { 0, -1, -1, -1, -1, 2 }, field);
            AssertEx.AreEqual(new int[] { 1, 1, 2 }, cellsPerProcessor);
        }

        [TestMethod]
        public void CrossoverFreePositionTest()
        {
            Random random = new Random(2014);

            Arr<int> field = new Arr<int>(new int[] { -1, -1, -1, -1, -1, -1 });
            for (int i = 0; i < 6; i++)
            {
                AssertEx.IsGreaterThanOrEqualTo(field.FreePosition(random.Next(0, 6 - i), -1), i);
                field[i] = 0;
            }

            field = new Arr<int>(new int[] { -1, -1, -1, -1, -1, -1 });
            for (int i = 0; i < 6; i++)
            {
                int position = field.FreePosition(random.Next(0, 6 - i), -1);
                field[position] = 0;
            }
            Assert.AreEqual(0, field.Sum);
        }

        [TestMethod]
        public void OverheadTest()
        {
            Assert.AreEqual(8, create(new int[] { 4, 5 }, new int[] { 0, 1, 0, 1, 1, 1, 0, 1, 0 }, 3, 3).Overhead(false));
            Assert.AreEqual(9, create(new int[] { 5, 4 }, new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0 }, 3, 3).Overhead(false));
            Assert.AreEqual(15, create(new int[] { 7, 9 }, new int[] { 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0 }, 4, 4).Overhead(false));

            Assert.AreEqual(13, create(new int[] { 4, 4, 1 }, new int[] { 0, 1, 0, 1, 2, 1, 0, 1, 0 }, 3, 3).Overhead(false));
            Assert.AreEqual(8, create(new int[] { 1, 1, 1, 1 }, new int[] { 0, 1, 2, 3 }, 2, 2).Overhead(false));

            Assert.AreEqual(24, create(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, 3, 3).Overhead(false));
            Assert.AreEqual(11, create(new int[] { 4, 2, 6 }, new int[] { 0, 0, 1, 1, 0, 0, 2, 2, 2, 2, 2, 2 }, 4, 3).Overhead(false));
            Assert.AreEqual(40, create(new int[] { 25, 25, 25, 25 }, new int[] { 
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
            }, 10, 10).Overhead(false));
            Assert.AreEqual(39, create(new int[] { 25, 25, 25, 25 }, new int[] { 
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
            }, 10, 10).Overhead(false));
            Assert.AreEqual(80, create(new int[] { 100, 100, 100, 100 }, new int[] { 
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
            }, 20, 20).Overhead(false));
        }

        [TestMethod]
        public void OverheadNewTest()
        {
            Assert.AreEqual(120, create(new int[] { 25, 25, 25, 25 }, new int[] { 
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
            }, 10, 10).Overhead(true));
            Assert.AreEqual(135, create(new int[] { 25, 25, 25, 25 }, new int[] { 
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
            }, 10, 10).Overhead(true));
            Assert.AreEqual(240, create(new int[] { 100, 100, 100, 100 }, new int[] { 
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
            }, 20, 20).Overhead(true));
        }

        [TestMethod]
        public void UniqueNeighborCountTest()
        {
            Assert.AreEqual(0, StencilSpeciesArr.UniqueNeighborCount(new Arr<int>(new int[] { 1, 2, 1, 2, 2, 2, 1, 2, 1 },3, 3), 1, 1));
            Assert.AreEqual(1, StencilSpeciesArr.UniqueNeighborCount(new Arr<int>(new int[] { 1, 2, 1, 2, 1, 2, 1, 2, 1 }, 3, 3), 1, 1));
        }

        [TestMethod]
        public void NoClonesAroundTest()
        {
            Random random = new Random(2014);

            SelectMutateCrossoverPopulation pool = new SelectMutateCrossoverPopulation(random, new StencilSpeciesArrCreator(random, 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 }), 64);

            for (int g = 0; g < 10; g++)
            {
                pool.Feed(1000);

                for (int i = 0; i < pool.Individuals.Count - 1; i++)
                    for (int j = i + 1; j < pool.Individuals.Count; j++)
                        if (pool.Individuals[i].Equals(pool.Individuals[j]))
                            throw new Exception("Clones!! " + i + " and " + j);
            }
        }
    }
}
