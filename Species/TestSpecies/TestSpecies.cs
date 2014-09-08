using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class TestSpecies : IEvolvable
    {
        double valueA = 0;
        double valueB = 0;
        double valueC = 0;

        public TestSpecies(double valueA, double valueB, double valueC) { this.valueA = valueA; this.valueB = valueB; this.valueC = valueC; }

        public IEvolvable Clone()
        {
            return new TestSpecies(valueA, valueB, valueC);
        }

        public void Mutate(Random random)
        {
            valueA += 10 * (random.NextDouble() - 0.5);
            valueB += 10 * (random.NextDouble() - 0.5);
            valueC += 10 * (random.NextDouble() - 0.5);
        }

        public IEvolvable Crossover(Random random, IEvolvable other)
        {
            return new TestSpecies(
                Evolution.RandomInterpolation(random, this.valueA, (other as TestSpecies).valueA),
                Evolution.RandomInterpolation(random, this.valueB, (other as TestSpecies).valueB),
                Evolution.RandomInterpolation(random, this.valueC, (other as TestSpecies).valueC));
        }

        public double Fitness
        {
            get { return Math.Min(valueA, 100011.0) + Math.Min(valueB, 1000.0) + Math.Min(valueC, 10100.0); }
        }

        public void Feed(Random random, int resources) { }

        public override string ToString()
        {
            return "Speciman with Fitness of " + Fitness;
        }
    }
}
