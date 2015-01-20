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

        private bool fitnessIsInValid = true;
        private double fitness;

        public virtual double Fitness
        {
            get
            {
                if (fitnessIsInValid)
                {
                    population.NoteFitnessEvaluations();
                    FitnessEvaluations++;
                    fitness = assessFitness();
                    fitnessIsInValid = false;
                }
                return fitness;
            }
        }

        public virtual long Mutations { get; protected set; }
        public virtual long Crossovers { get; protected set; }
        public virtual long FitnessEvaluations { get; protected set; }

        public Evolvable(IPopulation population)
        {
            this.population = population;
        }

        protected void resetFitness() { fitnessIsInValid = true; }

        protected double mutate(Random random, double value, double min, double max, double scale)
        {
            return Math.Min(max, Math.Max(min, value + (random.NextDouble() - 0.5) * scale));
        }

        public void Mutate()
        {
            population.NoteMutation();
            Mutations++;
            mutate();
            resetFitness();
        }

        public void Leap()
        {
            leap();
            resetFitness();
        }

        public IEvolvable Crossover(IEvolvable mate)
        {
            population.NoteCrossovers();
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
        protected abstract void leap();
        protected abstract IEvolvable crossover(IEvolvable mate);
        protected abstract double differenceTo(IEvolvable other);
        protected abstract IEvolvable clone();

        public override string ToString()
        {
            return "Fitness: " + Fitness + " Counts: F" + FitnessEvaluations + " M" + Mutations + " C" + Crossovers;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        public abstract bool IsValid { get; }
    }
}
