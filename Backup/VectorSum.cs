using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public class VectorSum : Algorithm
    {
        public VectorSum() : base(5, 0, 0) { }

        public override void Run()
        {
            Array<int> vector1 = Memory.GetArray<int>(sizeX);
            Array<int> vector3 = Memory.GetArray<int>(sizeX);
            Array<int> vector2 = Memory.GetArray<int>(sizeX);

            // init
            for (int i = 0; i < sizeX; i++)
            {
                vector1[i] = i;
                vector2[i] = 2 * i;
                vector3[i] = 0;
            }
            Memory.FinishRound();

            // round
            Parallel.For(0, sizeX, (int i) =>
            {
                vector3[i] = vector1[i] + vector2[i];
            });
            Memory.FinishRound();
        }
    }
}
