using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class BorderSwapMutator : Mutator
    {
        protected bool IncludeFieldBorders = true;

        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            int[] f = (int[])field.Clone();
            int validPositionCount = 0;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    if (8 > similarNeighbors(f, x, y, w, h, IncludeFieldBorders))
                    {
                        f[coords(x, y, w)] = -1;
                        validPositionCount++;
                    }
                }

            mutations = Math.Min(mutations, validPositionCount);
            for (int i = 0; i < mutations; i++)
                swap(field, freePosition(random, validPositionCount, f), freePosition(random, validPositionCount, f));
        }
    }
}
