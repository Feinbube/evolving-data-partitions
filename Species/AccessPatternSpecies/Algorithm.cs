using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public abstract class Algorithm<T> : IAlgorithm
    {
        public abstract Arr<byte> EmptyIndexRange();
        public abstract List<NotifyingArr<T>> Arrays();

        public RunResult Run(Arr<byte> indexRange, byte threadCount)
        {
            RunResult result = new RunResult(indexRange, Arrays().Select(a => (a as INotifyingArray)).ToList());

            for (byte tID = 0; tID < threadCount; tID++)
            {
                result.SetThreadID(tID);

                for (int indexZ = 0; indexZ < indexRange.SizeZ; indexZ++)
                    for (int indexY = 0; indexY < indexRange.SizeY; indexY++)
                        for (int indexX = 0; indexX < indexRange.SizeX; indexX++)
                        {
                            if (tID != indexRange[indexX, indexY, indexZ])
                                continue;

                            apply(indexX, indexY, indexZ);
                        }
            }

            result.Finish();

            return result;
        }

        protected abstract void apply(int indexX, int indexY, int indexZ);
        public abstract IAlgorithm Clone();
    }
}
