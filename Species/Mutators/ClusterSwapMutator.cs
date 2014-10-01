using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class ClusterSwapMutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int i = 0; i < mutations; i++)
            {
                int cw = random.Next(1, w - 2);
                int ch = random.Next(1, h - 2);
                int x1 = random.Next(0, w - cw);
                int y1 = random.Next(0, h - ch);
                int x2 = random.Next(0, w - cw);
                int y2 = random.Next(0, h - ch);

                for (int x = 0; x < cw; x++)
                    for (int y = 0; y < ch; y++)
                        swap(field, coords(x + x1, y + y1, w), coords(x + x2, y + y2, w));
            }
        }

        public override void Mutate(Random random, ExecutionEnvironment.Arr<int> field, int mutations)
        {
            for (int i = 0; i < mutations; i++)
            {
                int cw = random.Next(1, field.W - 2);
                int ch = random.Next(1, field.H - 2);
                int x1 = random.Next(0, field.W - cw);
                int y1 = random.Next(0, field.H - ch);
                int x2 = random.Next(0, field.W - cw);
                int y2 = random.Next(0, field.H - ch);

                field.SwapCluster(x1, y1, cw, ch, x2, y2);
            }
        }
    }
}
