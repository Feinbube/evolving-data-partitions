using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class DiagonalNeighborSwapMutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int i = 0; i < mutations; i++)
            {
                int positionX = random.Next(0, w);
                int positionY = random.Next(0, h);

                int diffX = random.Next(0, 2) == 0 ? -1 : 1;
                if (positionX == 0) diffX = 1;
                if (positionX == w - 1) diffX = -1;

                int diffY = random.Next(0, 2) == 0 ? -1 : 1;
                if (positionY == 0) diffY = 1;
                if (positionY == h - 1) diffY = -1;

                swap(field, coords(positionX, positionY, w), coords(positionX + diffX, positionY + diffY, w));
            }
        }
    }
}
