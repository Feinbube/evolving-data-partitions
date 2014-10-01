using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkFramework
{
    public class Benchmark
    {
        public BenchmarkResult Run(IBenchmarkable benchmark, object configuration)
        {
            DateTime start = DateTime.Now;
            double performance = benchmark.RunBenchmark(configuration);
            return new BenchmarkResult() { RunTime = DateTime.Now - start, Performance = performance };
        }

        public List<BenchmarkResult> Run(IBenchmarkable benchmark, List<object> configurations)
        {
            List<BenchmarkResult> result = new List<BenchmarkResult>();
            foreach(object configuration in configurations)
                result.Add(Run(benchmark, configuration));
            return result;
        }
    }
}
