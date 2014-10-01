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

        public int PopulationSize { get { return individuals == null ? 0 : individuals.Count; } }

        public long Generations { get; set; }

        public double Fitness { get { return Best.Fitness; } }

        protected IEvolvable bestCache = null;
        public virtual IEvolvable Best { get { if (bestCache == null) bestCache = individuals.MaxElement(a => a.Fitness); return bestCache; } }

        protected IEvolvable worstCache = null;
        public virtual IEvolvable Worst { get { if (worstCache == null) worstCache = individuals.MinElement(a => a.Fitness); return worstCache; } }

        public Population(ICreator creator, Random random, int populationSize) { construct(creator, random, null, populationSize, 0, 0, 0, 0); }

        public Population(ICreator creator, Random random, List<IEvolvable> individuals) { construct(creator, random, individuals, individuals.Count, 0, 0, 0, 0); }

        protected Population(ICreator creator, Random random, List<IEvolvable> individuals, int populationSize, long generations, long individualMutations, long individualCrossovers, long individualFitnessEvaluations, double foodConsumedInLifetime) : base(foodConsumedInLifetime) { construct(creator, random, individuals, populationSize, generations, individualMutations, individualCrossovers, individualFitnessEvaluations); }

        protected Population(Population original) : base(original) { construct(original.Creator, original.random, original.individuals, original.PopulationSize, original.Generations, original.IndividualMutations, original.IndividualCrossovers, original.IndividualFitnessEvaluations); }

        private void construct(ICreator creator, Random random, List<IEvolvable> individuals, int populationSize, long generations, long individualMutations, long individualCrossovers, long individualFitnessEvaluations)
        {
            this.Creator = creator;
            this.random = random;
            this.Generations = generations;
            this.IndividualMutations = individualMutations;
            this.IndividualCrossovers = individualCrossovers;
            this.IndividualFitnessEvaluations = individualFitnessEvaluations;

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

            sortIndividualsByFitness();
        }

        protected void invalidateCaches() { bestCache = null; worstCache = null; }

        protected void sortIndividualsByFitness() { this.individuals = this.individuals.OrderByDescending(a => a.Fitness).ToList(); }

        private Object individualMutationsLock = new Object();
        private Object individualCrossoversLock = new Object();
        private Object individualFitnessEvaluationsLock = new Object();

        public long IndividualMutations { get; protected set; }
        public long IndividualCrossovers { get; protected set; }
        public long IndividualFitnessEvaluations { get; protected set; }

        public virtual void NoteIndividualMutation() { lock (individualMutationsLock) IndividualMutations++; }
        public virtual void NoteIndividualCrossover() { lock (individualCrossoversLock) IndividualCrossovers++; }
        public virtual void NoteIndividualFitnessEvaluation() { lock (individualFitnessEvaluationsLock) IndividualFitnessEvaluations++; }

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
            return "Population: " + individuals.Count + " Generations: " + Generations + " Mutations: " + IndividualMutations + " Crossovers: " + IndividualCrossovers + " Evaluations: " + IndividualFitnessEvaluations;
        }
    }
}
