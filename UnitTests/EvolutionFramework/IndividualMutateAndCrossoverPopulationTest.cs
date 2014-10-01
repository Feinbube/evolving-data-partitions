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
    [TestClass]
    public class IndividualMutateAndCrossoverPopulationTest : PopulationTest
    {
        protected override IPopulation createTestPopulation(Random random)
        {
            return new IndividualMutateAndCrossoverPopulation(random, new TestCreator(random), 100);
        }

        protected override int reasonableFood()
        {
            return 5000;
        }
    }
}