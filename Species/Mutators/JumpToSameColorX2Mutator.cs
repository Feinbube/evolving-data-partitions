using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public class JumpToSameColorX2Mutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            throw new NotImplementedException();
        }

        public override void Mutate(Random random, ExecutionEnvironment.Arr<int> field, int mutations)
        {
            for (int m = 0; m < mutations; m++)
            {
                // remove invalid cells (cells with only neighbors of same color)
                ExecutionEnvironment.Arr<int> f = field.Clone();
                int validCount = 0;
                for (int y = 0; y < f.H; y++)
                    for (int x = 0; x < f.W; x++)
                        if (field.NeighborsWithValueHV(field[x, y], x, y, 1, true) == 4) f[x, y] = -1;
                        else validCount++;

                // choose start pos and color
                int startPos = f.ValidPosition(random.Next(0, validCount), -1);
                f[startPos] = -1;
                validCount--;
                int color = field[startPos];

                ExecutionEnvironment.Arr<int> f2 = f.Clone();
                // remove all cells that have no neighbor with matching color and cells with same color
                for (int y = 0; y < f.H; y++)
                    for (int x = 0; x < f.W; x++)
                        if (f[x, y] != -1 && (f[x, y] == color || f.NeighborsWithValueHV(color, x, y, 1, false) == 0))
                        {
                            f2[x, y] = -1;
                            validCount--;
                        }

                // select target at random
                int targetPos = f2.ValidPosition(random.Next(0, validCount), -1);

                // swap
                field.Swap(startPos, targetPos);
            }
        }
    }
}
