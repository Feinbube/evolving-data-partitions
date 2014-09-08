using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public class Convolution2 : Algorithm
    {
        public Convolution2() : base(4, 4, 5) { }

        public override void Run()
        {
            Array<double> startImage = Memory.GetArray<double>(sizeX, sizeY);
            Array<double> outImage = Memory.GetArray<double>(sizeX, sizeY);

            double EDGE = 5.0;

            // create random, directed weight matrix
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    startImage[i, j] = Random.Next(1, 100);
            Memory.FinishRound();

            //Set up edge values within border elements
            for (int k = 0; k < sizeX; k++)
            {
                startImage[k, 0] = outImage[k, 0] = EDGE;
                startImage[k, sizeY - 1] = outImage[k, sizeY - 1] = EDGE;
            }
            Memory.FinishRound();

            for (int k = 0; k < sizeY; k++)
            {
                startImage[0, k] = outImage[0, k] = EDGE;
                startImage[sizeX - 1, k] = outImage[sizeX - 1, k] = EDGE;
            }
            Memory.FinishRound();

            for (int rounds = 0; rounds < sizeZ; rounds++)
            {
                Parallel.For(1, sizeX - 1, 1, sizeY - 1, (int i, int j) =>
                {
                    outImage[i, j] = (int)((1.0 / 5.0) * (startImage[i, j] + startImage[i, j + 1] + startImage[i, j - 1] + startImage[i + 1, j] + startImage[i - 1, j]));
                });
                Memory.FinishRound();
            }
        }
    }
}
