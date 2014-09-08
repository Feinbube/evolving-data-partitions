using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public class MatrixMultiplication : Algorithm
    {
        public MatrixMultiplication() : base(3, 2, 0) { }

        public override void Run()
        {
            Array<int> mat1 = Memory.GetArray<int>(sizeX, sizeY);
            Array<int> mat3 = Memory.GetArray<int>(sizeY, sizeY);
            Array<int> mat2 = Memory.GetArray<int>(sizeY, sizeX);

            // init
            mat1[0, 0] = 3;
            mat1[1, 0] = 2;
            mat1[2, 0] = 1;
            mat1[0, 1] = 1;
            mat1[1, 1] = 0;
            mat1[2, 1] = 2;

            mat2[0, 0] = 1;
            mat2[1, 0] = 2;
            mat2[0, 1] = 0;
            mat2[1, 1] = 1;
            mat2[0, 2] = 4;
            mat2[1, 2] = 0;

            mat3[0, 0] = 0;
            mat3[1, 0] = 0;
            mat3[0, 1] = 0;
            mat3[1, 1] = 0;
            Memory.FinishRound();

            // round
            for (int j = 0; j < sizeX; j++)
            {
                Parallel.For(0, sizeY, (int x) =>
                {
                    Parallel.For(0, sizeY, (int y) =>
                       {
                           mat3[x, y] = mat3[x, y] + mat1[j, y] * mat2[x, j];
                       });
                });
                Memory.FinishRound();
            }
        }
    }
}
