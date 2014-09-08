﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class RowAndColumnSwapMutator : Mutator
    {
        public override void Mutate(Random random, int[] field, int w, int h, int mutations)
        {
            for (int i = 0; i < mutations; i++)
                if (random.Next(0, 2) == 0)
                    swapColumn(field, random.Next(0, w), random.Next(0, w), w, h);
                else
                    swapRow(field, random.Next(0, h), random.Next(0, h), w, h);
        }
    }
}
