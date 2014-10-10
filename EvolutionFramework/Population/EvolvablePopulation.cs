using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public abstract class EvolvablePopulation : Population, IFeedable, INotingEvolvable, ICreator
    {
        public IPopulation ParentPopulation { get; set; }

        public long EvolveMutations { get; protected set; }
        public long EvolveCrossovers { get; protected set; }
        public long EvolveFitnessEvaluations { get; protected set; }

        public override List<IEvolvable> Individuals { get { return base.Individuals.Select(a => (a as IEvolver).Evolvable).ToList(); } }
        public override List<IEvolvable> IndividualsSortedByFitness { get { return base.IndividualsSortedByFitness.Select(a => (a as IEvolver).Evolvable).ToList(); } }

        public override IEvolvable Best
        {
            get
            {
                if (bestCache == null)
                {
                    if (ParentPopulation != null)
                        ParentPopulation.NoteFitnessEvaluations();
                    EvolveFitnessEvaluations++;
                }
                return (base.Best as IEvolver).Evolvable;
            }
        }

        public IEvolvable BestEvolver { get { return base.Best; } }

        public override IEvolvable Worst { get { return (base.Worst as IEvolver).Evolvable; } }

        public IEvolvable WorstEvolver { get { return base.Worst; } }

        private List<double> fitnessHistory = new List<double>();
        public List<double> FitnessHistory { get { lock (fitnessHistory) { return fitnessHistory.Select(a => a).ToList(); } } }

        public EvolvablePopulation(IPopulation population, Random random, ICreator creator, int populationSize) : base(creator, random, populationSize) { construct(population, 0, 0, 0, null); }

        public EvolvablePopulation(IPopulation population, Random random, ICreator creator, List<IEvolvable> individuals) : base(creator, random, individuals) { construct(population, 0, 0, 0, null); }

        protected EvolvablePopulation(IPopulation parentPopulation, Random random, ICreator creator, int evolveMutations, int evolveCrossovers, int evolveFitnessEvaluations, List<double> fitnessHistory, List<IEvolvable> individuals, int populationSize, long generations, long mutations, long crossovers, long fitnessEvaluations, double foodConsumedInLifetime) : base(creator, random, individuals, populationSize, generations, mutations, crossovers, fitnessEvaluations, foodConsumedInLifetime) { construct(parentPopulation, evolveMutations, evolveCrossovers, evolveFitnessEvaluations, fitnessHistory); }

        public EvolvablePopulation(EvolvablePopulation original) : base(original) { construct(original.ParentPopulation, original.Mutations, original.Crossovers, original.FitnessEvaluations, original.fitnessHistory); }

        void construct(IPopulation parentPopulation, long evolveMutations, long evolveCrossovers, long evolveFitnessEvaluations, List<double> fitnessHistory)
        {
            this.ParentPopulation = parentPopulation;

            this.EvolveMutations = evolveMutations;
            this.EvolveCrossovers = evolveCrossovers;
            this.EvolveFitnessEvaluations = evolveFitnessEvaluations;

            if (fitnessHistory != null)
                this.fitnessHistory = fitnessHistory.Select(a => a).ToList();
        }

        protected double mutate(double value, double min, double max, double scale)
        {
            return Math.Min(max, Math.Max(min, value + (random.NextDouble() - 0.5) * scale));
        }

        public double FitnessDelta(int numberOfRoundsBack)
        {
            if (fitnessHistory.Count < numberOfRoundsBack)
                return 1;

            double result = Fitness - fitnessHistory[0];
            for (int i = 1; i < numberOfRoundsBack; i++)
                result += fitnessHistory[i - 1] - fitnessHistory[i];
            return result;
        }

        protected void measureFitness()
        {
            lock (fitnessHistory)
            {
                fitnessHistory.Insert(0, Fitness);
                if (fitnessHistory.Count > 100)
                    fitnessHistory.RemoveAt(100);
            }
        }

        public void Mutate()
        {
            if (ParentPopulation != null)
                ParentPopulation.NoteMutation();
            EvolveMutations++;
            mutate();
            invalidateCaches();
        }

        public IEvolvable Crossover(IEvolvable mate)
        {
            if (ParentPopulation != null)
                ParentPopulation.NoteCrossovers();
            EvolveCrossovers++;
            return crossover(mate);
        }

        protected abstract void mutate();

        protected abstract IEvolvable crossover(IEvolvable other);

        public abstract IEvolvable Clone();

        public abstract double DifferenceTo(IEvolvable other);

        public override IEvolvable Create()
        {
            IEvolvable result = base.Create();

            if ((result as Evolver).Evolvable is EvolvablePopulation)
                ((result as Evolver).Evolvable as EvolvablePopulation).ParentPopulation = this;

            return result;
        }

        public override void NoteMutation() { base.NoteMutation(); if (ParentPopulation != null) ParentPopulation.NoteMutation(); }

        public override void NoteCrossovers() { base.NoteCrossovers(); if (ParentPopulation != null) ParentPopulation.NoteCrossovers(); }

        public override void NoteFitnessEvaluations() { base.NoteFitnessEvaluations(); if (ParentPopulation != null) ParentPopulation.NoteFitnessEvaluations(); }
    }
}
