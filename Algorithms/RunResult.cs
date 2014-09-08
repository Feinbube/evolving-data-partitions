using System;
using System.Collections.Generic;
using ExecutionEnvironment;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class RunResult
    {
        public string Name;

        public double SizeX;
        public double SizeY;
        public double SizeZ;

        public bool Valid;
        public double ElapsedTotalSeconds;

        public double RelativeExecutionTime(double other)
        {
            if (!Valid)
                return -1;

            return other / this.ElapsedTotalSeconds;
        }
    }
}