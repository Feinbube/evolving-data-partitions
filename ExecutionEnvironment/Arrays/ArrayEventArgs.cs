using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class ArrayEventArgs : EventArgs
    {
        public int ThreadId { get; private set; }
        public int Index1D { get; private set; }

        public ArrayEventArgs(int threadId, int index1D)
        {
            this.ThreadId = threadId;
            this.Index1D = index1D;
        }
    }
}
