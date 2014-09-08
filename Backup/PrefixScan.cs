using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public class PrefixScan : Algorithm
    {
        public PrefixScan() : base(10, 0, 0) { }

        public override void Run()
        {
            Array<int> startData = Memory.GetArray<int>(sizeX);
            Array<int> IscanData = Memory.GetArray<int>(sizeX);

            for (int i = 0; i < sizeX; i++)
                startData[i] = Random.Next(1, 100);
            Memory.FinishRound();

            int increment = 1;

            Array<int> p1 = startData;
            Array<int> p2 = IscanData;
            Array<int> ptmp;

            IscanData[0] = startData[0];

            while (increment < sizeX)
            {

                Parallel.For(1, increment, delegate(int i)
                {
                    p2[i] = p1[i];
                });
                Memory.FinishRound();

                Parallel.For(increment, sizeX, delegate(int i)
                {
                    p2[i] = p1[i] + p1[i - increment];
                });
                Memory.FinishRound();

                // increment
                increment = increment << 1;

                // switch arrays
                ptmp = p1;
                p1 = p2;
                p2 = ptmp;
            }
        }
    }
}
