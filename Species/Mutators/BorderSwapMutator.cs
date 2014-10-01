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

            mutations = Math.Min(mutations, validPositionCount);
            for (int i = 0; i < mutations; i++)
                swap(field, freePosition(random, validPositionCount, f), freePosition(random, validPositionCount, f));
        }

        public override void Mutate(Random random, ExecutionEnvironment.Arr<int> field, int mutations)
        {
            ExecutionEnvironment.Arr<int> f = field.Clone();
            int validPositionCount = 0;
            int pos = 0;
            for (int x = 0; x < field.W; x++)
                for (int y = 0; y < field.H; y++)
                {
                    if (8 > f.SimilarNeighbors(x, y, IncludeFieldBorders))
                    {
                        f[pos] = -1;
                        validPositionCount++;
                    }
                    pos++;
                }

            mutations = Math.Min(mutations, validPositionCount);
            for (int i = 0; i < mutations; i++)
                field.Swap(f.FreePosition(random.Next(0, validPositionCount), -1), f.FreePosition(random.Next(0, validPositionCount), -1));
        }
    }
}
