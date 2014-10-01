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
                int pos = 0;
                for (int x = 0; x < w; x++)
                    for (int y = 0; y < h; y++)
                    {
                        if (8 > similarNeighbors(f, x, y, w, h, IncludeFieldBorders))
                        {
                            f[pos] = -1;
                            validPositionCount++;
                        }
                        pos++;
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

        public override void Mutate(Random random, ExecutionEnvironment.Arr<int> field, int mutations)
        {
            for (int i = 0; i < mutations; i++)
            {
                ExecutionEnvironment.Arr<int> f = field.Clone();
                int validPositionCount = 0;
                int pos = 0;
                for (int y = 0; y < field.H; y++)
                    for (int x = 0; x < field.W; x++)
                    {
                        if (8 > f.SimilarNeighbors(x, y, IncludeFieldBorders))
                        {
                            f[pos] = -1;
                            validPositionCount++;
                        }
                        pos++;
                    }

                int steps = random.Next(1, validPositionCount);
                int position = f.FreePosition(random.Next(0, validPositionCount), -1);
                int xPos = position % field.W;
                int yPos = position / field.W;
                int processor = f[position];
                for (int i2 = 0; i2 < steps; i2++)
                {
                    f[position] = -1;
                    int next = f.NextPosition(random, processor, -1, xPos, yPos);
                    if (next == -1)
                        break;
                    field.Swap(position, next);
                    position = next;
                }
            }
        }
    }
}
