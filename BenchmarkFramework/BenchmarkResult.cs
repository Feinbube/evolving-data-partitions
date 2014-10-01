using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkFramework
{
    public class BenchmarkResult
    {
        public double Performance { get; set; }
        public TimeSpan RunTime { get; set; }
    }
}
