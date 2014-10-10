using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQExtensions;

namespace EvolutionFramework
{
    public abstract class Population : Feedable, IPopulation, ICreator
    {
        public ICreator Creator { get; protected set; }

        protected Random random { get; set; }

        protected List<IEvolvable> individuals;

        public virtual List<IEvolvable> Individuals { get { return individuals; } }

        private List<IEvolvable> individualsSortedByFitnessCache = null;
        public virtual List<IEvolvable> IndividualsSortedByFitness { get { if (individualsSortedByFitnessCache == null) individualsSortedByFitnessCache = this.individuals.OrderByDescending(a => a.Fitness).ToList(); return individualsSortedByFitnessCache; } }

        public int PopulationSize { get { return individuals == null ? 0 : individuals.Count; } }

        public long Generations { get; set; }

        public double Fitness { get { return Best.Fitness; } }

        public long Mutations { get; protected set; }
        public long Crossovers { get; protected set; }
        public long FitnessEvaluations { get; protected set; }

        protected IEvolvable bestCache = null;
        public virtual IEvolvable Best { get { if (bestCache == null) bestCache = individuals.MaxElement(a => a.Fitness); return bestCache; } }

        protected IEvolvable worstCache = null;
        public virtual IEvolvable Worst { get { if (worstCache == null) worstCache = individuals.MinElement(a => a.Fitness); return worstCache; } }

        public Population(ICreator creator, Random random, int populationSize) { construct(creator, random, null, populationSize, 0, 0, 0, 0); }

        public Population(ICreator creator, Random random, List<IEvolvable> individuals) { construct(creator, random, individuals, individuals.Count, 0, 0, 0, 0); }

        protected Population(ICreator creator, Random random, List<IEvolvable> individuals, int populationSize, long generations, long mutations, long crossovers, long fitnessEvaluations, double foodConsumedInLifetime) : base(foodConsumedInLifetime) { construct(creator, random, individuals, populationSize, generations, mutations, crossovers, fitnessEvaluations); }

        protected Population(Population original) : base(original) { construct(original.Creator, original.random, original.individuals, original.PopulationSize, original.Generations, original.Mutations, original.Crossovers, original.FitnessEvaluations); }

        private void construct(ICreator creator, Random random, List<IEvolvable> individuals, int populationSize, long generations, long mutations, long crossovers, long fitnessEvaluations)
        {
            this.Creator = creator;
            this.random = random;
            this.Generations = generations;

            this.Mutations = mutations;
            this.Crossovers = crossovers;
            this.FitnessEvaluations = fitnessEvaluations;

            if (individuals != null)
            {
                this.individuals = new List<IEvolvable>();
                for (int i = 0; i < individuals.Count; i++)
                    this.individuals.Add(individuals[i].Clone());
            }
            else
            {
                this.individuals = new List<IEvolvable>();
                for (int i = 0; i < (int)populationSize; i++)
                    this.individuals.Add(Create());
            }

            invalidateCaches();
        }

        protected void invalidateCaches() { bestCache = null; worstCache = null; individualsSortedByFitnessCache = null; }

        public void Populate(int population)
        {
            for (int i = 0; i < population; i++)
                this.individuals.Add(this.Create());
            invalidateCaches();
        }

        public void Add(IEvolvable individual)
        {
            this.individuals.Add(individual);
            invalidateCaches();
        }

        public virtual IEvolvable Create() { return new Evolver(this) { Evolvable = Creator.Create() }; }

        public override string ToString()
        {
            return "Population: " + individuals.Count + " Generations: " + Generations + " Mutations: " + Mutations + " Crossovers: " + Crossovers + " Evaluations: " + FitnessEvaluations;
        }

        private object mutationsLock = new object();
        private object crossoversLock = new object();
        private object fitnessEvaluationsLock = new object();

        public virtual void NoteMutation() { lock (mutationsLock) { Mutations++; } }

        public virtual void NoteCrossovers() { lock (crossoversLock) { Crossovers++; } }

        public virtual void NoteFitnessEvaluations() { lock (fitnessEvaluationsLock) { FitnessEvaluations++; } }

    }
}
