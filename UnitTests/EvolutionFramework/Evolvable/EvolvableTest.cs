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
    public abstract class EvolvableTest
    {
        [TestMethod]
        public void MutationTest()
        {
            Random random = new Random(2014);
            ICreator creator = getCreator(random);

            for (int i = 0; i < 100; i++)
            {
                var specimen = creator.Create();
                Assert.IsTrue(specimen.IsValid);

                var clone = specimen.Clone();
                Assert.IsTrue(clone.IsValid);

                double cloneFitness = clone.Fitness;

                for (int i2 = 0; i2 < 100; i2++)
                {
                    specimen.Mutate();
                    Assert.IsTrue(specimen.IsValid);
                }

                Assert.AreNotEqual(specimen.Fitness, clone.Fitness);
                Assert.AreEqual(cloneFitness, clone.Fitness);
            }
        }

        protected abstract ICreator getCreator(Random random);
    }
}
