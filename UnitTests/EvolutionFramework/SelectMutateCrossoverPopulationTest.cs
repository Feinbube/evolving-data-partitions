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
    public class SelectMutateCrossoverPopulationTest : PopulationTest
    {
        protected override IPopulation createTestPopulation(Random random)
        {
            return new SelectMutateCrossoverPopulation(null, random, new TestCreator(random), 100);
        }

        protected override int reasonableFood()
        {
            return 10000;
        }

        [TestMethod]
        public void NoClonesAroundTest()
        {
            Random random = new Random(2014);

            SelectMutateCrossoverPopulation pool = new SelectMutateCrossoverPopulation(null, random, new TestCreator(random), 25);

            for (int g = 0; g < 10; g++)
            {
                pool.Feed(10000);

                for (int i = 0; i < pool.Individuals.Count - 1; i++)
                    for (int j = i + 1; j < pool.Individuals.Count; j++)
                        if (pool.Individuals[i].Equals(pool.Individuals[j]))
                            throw new Exception("Clones!! " + i + " and " + j);
            }
        }
    }
}
