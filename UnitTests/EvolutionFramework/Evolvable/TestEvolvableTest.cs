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
    public class TestEvolvableTest : EvolvableTest
    {
        protected override ICreator getCreator(Random random)
        {
            return new TestCreator(random);
        }
    }
}