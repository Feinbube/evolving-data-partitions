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
    public class EvolutionTest : HierarchicalPopulationTest
    {
        protected override IPopulation createTestPopulation(Random random)
        {
            return new Evolution(random, new TestCreator(random), 7, 64) { EnoughFeedingsForBreeding = 1000 };
        }

        protected override int reasonableFood()
        {
            return 30000;
        }

        public override void TypeOfChildrenTest() { }
    }
}