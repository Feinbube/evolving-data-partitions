using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class TestSpeciesCreator : ICreator
    {
        public IEvolvable Create()
        {
            return new TestSpecies(0, 0, 0);
        }
    }
}
