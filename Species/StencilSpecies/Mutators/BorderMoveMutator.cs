using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class BorderMoveMutator : Mutator
    {
        protected bool IncludeFieldBorders = true;

        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int i = 0; i < mutations; i++)
            {
                int[] f = (int[])field.Clone();
                int validPositionCount = 0;
                for (int x = 0; x < w; x++)
                    for (int y = 0; y < h; y++)
                        if (8 > similarNeighbors(f, x, y, w, h, IncludeFieldBorders))
                        {
                            f[coords(x, y, w)] = -1;
                            validPositionCount++;
                        }

                int steps = random.Next(1, validPositionCount);
                int position = freePosition(random, validPositionCount, f);
                int processor = f[position];
                for (int i2 = 0; i2 < steps; i2++)
                {
                    f[position] = -1;
                    int next = nextPosition(random, processor, f, w, h, position);
                    if (next == -1)
                        break;
                    swap(field, position, next);
                    position = next;
                }
            }
        }
    }
}
