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
        }

        private void Run2()
        {
            throw new NotImplementedException();
            /*
            StencilSpeciesPopulation2 population = new StencilSpeciesPopulation2(100, 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 });
            TimeSpan maxRuntime = new TimeSpan(0, 0, 10);

            DateTime programStart = DateTime.Now;

            while (true)
            {
                DateTime start = DateTime.Now;

                while (DateTime.Now - start < maxRuntime)
                    population.Feed(123);

                Console.WriteLine();
                Console.WriteLine("Runtime: " + (DateTime.Now - programStart).ToString());
                Console.WriteLine(population.ToString() );
                Console.WriteLine("Best: " + population.Best.ToString());
            }
            */ 
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

                while (DateTime.Now - start < maxRuntime)
                    evolution.Feed(1000);
                
                Console.WriteLine();
                Console.WriteLine(DateTime.Now - programStart);
                Console.WriteLine(evolution.Best.ToString());
            }
        }

        private void Run()
        {
            throw new NotImplementedException();
            /*
            for (int population = 1; population <= 512; population *= 2)
            {
                ICreator creator = new StencilSpeciesCreator(random, 20, 20, new double[] { 0.25, 0.25, 0.25, 0.25 });
                TimeSpan maxRuntime = new TimeSpan(0, 0, 10);

                Console.WriteLine(population + " ==");
                for (int n = 1; n <= 10; n++)
                {
                    Console.Write(n + ": ");

                    //Parallel.For(1, 11, i =>
                    for (int i = 1; i <= 10; i++)
                    {
                        DateTime start = DateTime.Now;

                        Evolution evolution = new Evolution(random, creator, n, population) { };
                        while (DateTime.Now - start < maxRuntime)
                            evolution.Feed(1000);
                        Console.Write(evolution.Fitness + " ");
                        Console.WriteLine();
                        Console.WriteLine(evolution.Best.ToString());
                    }
                    //);
                    Console.WriteLine();
                }
            }
             */ 
        }
    }
}
