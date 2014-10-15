using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionFramework;
using BenchmarkFramework;
using DataFieldLayoutSimulation;

namespace BenchmarkConsole
{
    class Program
    {
        Random random = new Random();
        Benchmark benchmark = new Benchmark();

        static void Main(string[] args)
        {
            new Program().RunContinious();

            new Program().MeasureRun();
            new Program().MeasureRun();
            new Program().MeasureRun();
        }

        private void RunContinious()
        {
            ICreator creator = new StencilSpeciesArrCreator(random, 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 });
            Evolution evolution = new Evolution(random, creator, 7, 64) { };
            TimeSpan maxRuntime = new TimeSpan(0, 0, 10);

            DateTime programStart = DateTime.Now;

            while(true)
            {
                DateTime start = DateTime.Now;

                Console.WriteLine("Runtime: " + (DateTime.Now - programStart).ToString());
                Console.WriteLine("Best: " + evolution.Best.ToString());
                Console.WriteLine();

                while (DateTime.Now - start < maxRuntime)
                    evolution.Feed(1000);
            }
        }

        private void MeasureRun()
        {
            Random random = new Random(2014);
            ICreator creator = new StencilSpeciesArrCreator(random, 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 });
            Evolution evolution = new Evolution(random, creator, 7, 64) { };

            Console.WriteLine("Best: " + evolution.Best.ToString());
            Console.WriteLine();

            DateTime start = DateTime.Now;
            evolution.Feed(20000);
            Console.WriteLine();
            Console.WriteLine("Runtime: " + (DateTime.Now - start).ToString());
            Console.WriteLine("Best: " + evolution.Best.ToString());
        }
    }
}
