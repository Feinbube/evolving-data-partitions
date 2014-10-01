using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class JumpToSameColorMutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int m = 0; m < mutations; m++)
            {
                int[] f = (int[])field.Clone();
                int startPos = random.Next(0, w * h);
                int color = f[startPos];

                int validPositionCount = markField(f, color, w, h, startPos);
                swap(field, startPos, freePosition(random, validPositionCount, f));
            }
        }

        public static int markField(int[] f, int color, int w, int h, int startpos)
        {
            int[] field = (int[])f.Clone();
            field[startpos] = -1;
            int validPositionCount = 0;
            int pos = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    if (pos != startpos && field[pos] != color && 0 < neighborsWithColorHV(color, field, x, y, w, h, false))
                    {
                        f[pos] = -1;
                        validPositionCount++;
                    }
                    pos++;
                }
            return validPositionCount;
        }

        public override void Mutate(Random random, ExecutionEnvironment.Arr<int> field, int mutations)
        {
            for (int m = 0; m < mutations; m++)
            {
                ExecutionEnvironment.Arr<int> f = field.Clone();
                int startPos = random.Next(0, field.Length);
                int color = f[startPos];

                int validPositionCount = markField(f, color, startPos);
                field.Swap(startPos, f.FreePosition(random.Next(0, validPositionCount), -1));
            }
        }

        public static int markField(ExecutionEnvironment.Arr<int> f, int color, int startpos)
        {
            ExecutionEnvironment.Arr<int> field = f.Clone();
            field[startpos] = -1;
            int validPositionCount = 0;
            int pos = 0;
            for (int y = 0; y < f.H; y++)
                for (int x = 0; x < f.W; x++)
                {
                    if (pos != startpos && field[pos] != color && 0 < field.NeighborsWithValueHV(color, x, y, 1, false))
                    {
                        f[pos] = -1;
                        validPositionCount++;
                    }
                    pos++;
                }
            return validPositionCount;
        }
    }
}
