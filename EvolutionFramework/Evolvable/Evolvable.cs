using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public abstract class Evolvable : INotingEvolvable
    {
        protected IPopulation population { get; set; }

        private double fitness = double.NaN;

        public double Fitness
        {
            get
            {
                if (fitness.Equals(double.NaN))
                {
                    fitness = assessFitness();
                    FitnessEvaluations++;
                    population.NoteIndividualFitnessEvaluation();
                }
                return fitness;
            }
        }

        public int Mutations { get; protected set; }
        public int Crossovers { get; protected set; }
        public int FitnessEvaluations { get; protected set; }

        public Evolvable(IPopulation population)
        {
            this.population = population;
        }

        protected void resetFitness() { fitness = double.NaN; }

        protected double mutate(Random random, double value, double min, double max, double scale)
        {
            return Math.Min(max, Math.Max(min, value + (random.NextDouble() - 0.5) * scale));
        }

        public void Mutate()
        {
            population.NoteIndividualMutation();
            Mutations++;
            mutate();
            resetFitness();
        }

        public IEvolvable Crossover(IEvolvable mate)
        {
            population.NoteIndividualCrossover();
            Crossovers++;
            return crossover(mate);
        }

        public double DifferenceTo(IEvolvable other)
        {
            return differenceTo(other);
        }

        public IEvolvable Clone()
        {
            return clone();
        }

        protected abstract double assessFitness();
        protected abstract void mutate();
        protected abstract IEvolvable crossover(IEvolvable mate);
        protected abstract double differenceTo(IEvolvable other);
        protected abstract IEvolvable clone();

        public override string ToString()
        {
            return "Fitness: " + Fitness + " Mutations: " + Mutations;
        }
    }
}
