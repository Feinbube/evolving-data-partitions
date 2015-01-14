using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public interface IAlgorithm
    {
        Arr<byte> EmptyIndexRange();
        RunResult Run(Arr<byte> indexRange, byte threadCount);

        IAlgorithm Clone();
    }
}
