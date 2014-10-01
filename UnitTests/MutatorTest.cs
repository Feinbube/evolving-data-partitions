using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFieldLayoutSimulation;
using EvolutionFramework;
using UnitTestHelpers;
using ExecutionEnvironment;

namespace UnitTests
{
    [TestClass]
    public class MutatorTest
    {
        [TestMethod]
        public void NeighborsWithColorTest()
        {
            Assert.AreEqual(0, Mutator.neighborsWithColorHV(1, new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }, 1, 1, 3, 3, false));
            Assert.AreEqual(4, Mutator.neighborsWithColorHV(1, new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 1, 1, 3, 3, false));
            Assert.AreEqual(1, Mutator.neighborsWithColorHV(1, new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 }, 1, 1, 3, 3, false));
            Assert.AreEqual(1, Mutator.neighborsWithColorHV(1, new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 }, 0, 0, 3, 3, false));

            Assert.AreEqual(0, new Arr<int>(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }, 3, 3).NeighborsWithValueHV(1, 1, 1, 1, false));
            Assert.AreEqual(4, new Arr<int>(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 3, 3).NeighborsWithValueHV(1, 1, 1, 1, false));
            Assert.AreEqual(1, new Arr<int>(new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 }, 3, 3).NeighborsWithValueHV(1, 1, 1, 1, false));
            Assert.AreEqual(1, new Arr<int>(new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 }, 3, 3).NeighborsWithValueHV(0, 0, 1, 1, false));
        }

        [TestMethod]
        public void JumpToSameColorMutatorMarkFieldTest()
        {
            int[] f = new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            Assert.AreEqual(0, JumpToSameColorMutator.markField(f, 1, 3, 3, 4));
            AssertEx.AreEqual(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }, f);

            f = new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 };
            Assert.AreEqual(2, JumpToSameColorMutator.markField(f, 1, 3, 3, 4));
            AssertEx.AreEqual(new int[] { 1, 1, -1, -1, 1, 0, 0, 0, 0 }, f);

            f = new int[] { 1, 1, 0, 2, 1, 2, 2, 2, 2 };
            Assert.AreEqual(2, JumpToSameColorMutator.markField(f, 1, 3, 3, 4));
            AssertEx.AreEqual(new int[] { 1, 1, -1, -1, 1, 2, 2, 2, 2 }, f);

            f = new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            Assert.AreEqual(1, JumpToSameColorMutator.markField(f, 0, 3, 3, 0));
            AssertEx.AreEqual(new int[] { 0, 0, 0, 0, -1, 0, 0, 0, 0 }, f);

            Arr<int> f2 = new Arr<int>(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }, 3, 3);
            Assert.AreEqual(0, JumpToSameColorMutator.markField(f2, 1, 4));
            AssertEx.AreEqual(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }, f2);

            f2 = new Arr<int>(new int[] { 1, 1, 0, 0, 1, 0, 0, 0, 0 }, 3, 3);
            Assert.AreEqual(2, JumpToSameColorMutator.markField(f2, 1, 4));
            AssertEx.AreEqual(new int[] { 1, 1, -1, -1, 1, 0, 0, 0, 0 }, f2);

            f2 = new Arr<int>(new int[] { 1, 1, 0, 2, 1, 2, 2, 2, 2 }, 3, 3);
            Assert.AreEqual(2, JumpToSameColorMutator.markField(f2, 1, 4));
            AssertEx.AreEqual(new int[] { 1, 1, -1, -1, 1, 2, 2, 2, 2 }, f2);

            f2 = new Arr<int>(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }, 3, 3);
            Assert.AreEqual(1, JumpToSameColorMutator.markField(f2, 0, 0));
            AssertEx.AreEqual(new int[] { 0, 0, 0, 0, -1, 0, 0, 0, 0 }, f2);

            f2 = new Arr<int>(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 1 }, 3, 3);
            Assert.AreEqual(2, JumpToSameColorMutator.markField(f2, 1, 0));
            AssertEx.AreEqual(new int[] { 1, 0, 0, 0, 0, -1, 0, -1, 1 }, f2);

            f2 = new Arr<int>(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 1 }, 3, 3);
            Assert.AreEqual(2, JumpToSameColorMutator.markField(f2, 1, 8));
            AssertEx.AreEqual(new int[] { 1, -1, 0, -1, 0, 0, 0, 0, 1 }, f2);
        }

        [TestMethod]
        public void JumpToSameColorX2MutatorTest()
        {
            Random random = new Random(2014);

            var fs = new List<int[]> {
                new int[] { 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 0 },
                new int[] { 1, 0, 0, 0, 0, 1, 0, 0, 0 },
                new int[] { 1, 0, 0, 0, 0, 0, 0, 1, 0 },

                new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 1 },
                new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 1 },
                new int[] { 0, 0, 0, 0, 0, 1, 0, 0, 1 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1 }
            };

            for (int i = 0; i < 1000; i++)
            {
                Arr<int> f = new Arr<int>(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 1 }, 3, 3);
                new JumpToSameColorX2Mutator().Mutate(random, f, 1);
                AssertEx.EqualToOne(fs, f);
            }
        }
    }
}
