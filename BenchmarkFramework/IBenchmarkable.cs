using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkFramework
{
    public interface IBenchmarkable
    {
        double RunBenchmark(object configuration);
    }
}
