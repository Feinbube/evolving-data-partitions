using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public interface IMemoryPlotter
    {
        void NewRound();
        void Plot(List<IArray> arrays);
    }
}
