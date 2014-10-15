using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionFramework;

namespace UnitTests
{
    public class TestCreator : ICreator
    {
        Random random;

        public TestCreator(Random random)
        {
            this.random = random;
        }

        public IEvolvable Create()
        {
            return new TestEvolvable(random, 100);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
