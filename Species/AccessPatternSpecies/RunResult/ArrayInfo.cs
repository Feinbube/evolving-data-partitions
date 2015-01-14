using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public class ArrayInfo
    {
        public Dictionary<byte, Arr<int>> ReadAccessesByThread;
        public Dictionary<byte, Arr<int>> WriteAccessesByThread;
    }
}
