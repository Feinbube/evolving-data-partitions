using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionFramework;
using BenchmarkFramework;
using Species;

namespace BenchmarkConsole
{
    class Program
    {
        
        Benchmark benchmark = new Benchmark();

        static void Main(string[] args)
        {
            Random random = new Random();
            ICreator creator = new StencilSpeciesArrCreator(random, 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 });
            IPopulation population = new IndividualMutateAndCrossoverPopulation(null, random, creator, 128) { };
            //IPopulation population = new SelectMutateCrossoverPopulation(null, random, creator, 128) { };
            //IPopulation population = new Evolution(random, creator, 7, 64) { };

            new Program().RunContinious(population);

            new Program().MeasureRun();
            new Program().MeasureRun();
            new Program().MeasureRun();
        }

        private void RunContinious(IPopulation population)
        {
            TimeSpan maxRuntime = new TimeSpan(0, 0, 10);

            DateTime programStart = DateTime.Now;

            while (true)
            {
                DateTime start = DateTime.Now;

                Console.WriteLine("Runtime: " + (DateTime.Now - programStart).ToString());
                Console.WriteLine("Best: " + population.Best.ToString());
                Console.WriteLine();

                while (DateTime.Now - start < maxRuntime)
                    population.Feed(1000);
            }
        }

        private void MeasureRun()
        {
            Random random = new Random(2014);
            ICreator creator = new StencilSpeciesArrCreator(random, 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 });
            Evolution population = new Evolution(random, creator, 7, 64) { };

            Console.WriteLine("Best: " + population.Best.ToString());
            Console.WriteLine();

            DateTime start = DateTime.Now;
            population.Feed(20000);
            Console.WriteLine();
            Console.WriteLine("Runtime: " + (DateTime.Now - start).ToString());
            Console.WriteLine("Best: " + population.Best.ToString());
        }
    }
}
