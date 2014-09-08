using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class CellSwapMutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int i = 0; i < mutations; i++)
                swap(field, random.Next(0, field.Length), random.Next(0, field.Length));
        }
    }
}
