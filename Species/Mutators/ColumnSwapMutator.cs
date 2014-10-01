using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class ColumnSwapMutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int i = 0; i < mutations; i++)
                swapColumn(field, random.Next(0, w), random.Next(0, w), w, h);
        }

        public override void Mutate(Random random, ExecutionEnvironment.Arr<int> field, int mutations)
        {
            for (int i = 0; i < mutations; i++)
                field.SwapColumn(random.Next(0, field.W), random.Next(0, field.W));
        }
    }
}
