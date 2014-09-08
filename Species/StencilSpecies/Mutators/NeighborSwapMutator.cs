using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class NeighborSwapMutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int i = 0; i < mutations; i++)
            {
                int positionX = random.Next(0, w);
                int positionY = random.Next(0, h);
                int direction = random.Next(0, 2) == 0 ? -1 : 1;
                if (random.Next(0, 2) == 0)
                { // horizontal
                    if (positionX == 0) direction = 1;
                    if (positionX == w- 1) direction = -1;
                    swap(field, coords(positionX, positionY, w), coords(positionX + direction, positionY, w));
                }
                else
                { // vertical
                    if (positionY == 0) direction = 1;
                    if (positionY == h - 1) direction = -1;
                    swap(field, coords(positionX, positionY, w), coords(positionX, positionY + direction, w));
                }
            }
        }
    }
}
