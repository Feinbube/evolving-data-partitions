using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionBenchmark
{
    public class Benchmark
    {
        public class BenchmarkResult
        {
            public double BestFitness { get; set; }
            public TimeSpan Runtime { get; set; }
        }
            
        public void Bench(TimeSpan runtime, int rounds)
        {

        }
    }
}
