using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public abstract class EvolvablePopulation : Population, IFeedable, INotingEvolvable, ICreator
    {
        public IPopulation Population { get; set; }

        public int Mutations { get; protected set; }
        public int Crossovers { get; protected set; }
        public int FitnessEvaluations { get; protected set; }

        public override List<IEvolvable> Individuals { get { return base.individuals.Select(a => (a as IEvolver).Evolvable).ToList(); } }

        public override IEvolvable Best
        {
            get
            {
                if (bestCache == null)
                {
                    FitnessEvaluations++;
                    if (Population != null)
                        Population.NoteIndividualFitnessEvaluation();
                }
                return (base.Best as IEvolver).Evolvable;
            }
        }

        public IEvolvable BestEvolver { get { return base.Best; } }

        public override IEvolvable Worst { get { return (base.Worst as IEvolver).Evolvable; } }

        public IEvolvable WorstEvolver { get { return base.Worst; } }

        private List<double> fitnessHistory = new List<double>();
        public List<double> FitnessHistory { get { lock (fitnessHistory) { return fitnessHistory.Select(a => a).ToList(); } } }

        public EvolvablePopulation(Random random, ICreator creator, int populationSize) : base(creator, random, populationSize) { construct(0, 0, 0, null); }

        public EvolvablePopulation(Random random, ICreator creator, List<IEvolvable> individuals) : base(creator, random, individuals) { construct(0, 0, 0, null); }

        protected EvolvablePopulation(Random random, ICreator creator, int mutations, int crossovers, int fitnessEvaluations, List<double> fitnessHistory, List<IEvolvable> individuals, int populationSize, long generations, long individualMutations, long individualCrossovers, long individualFitnessEvaluations, double foodConsumedInLifetime) : base(creator, random, individuals, populationSize, generations, individualMutations, individualCrossovers, individualFitnessEvaluations, foodConsumedInLifetime) { construct(mutations, crossovers, fitnessEvaluations, fitnessHistory); }

        public EvolvablePopulation(EvolvablePopulation original) : base(original) { construct(original.Mutations, original.Crossovers, original.FitnessEvaluations, original.fitnessHistory); }

        void construct(int mutations, int crossovers, int fitnessEvaluations, List<double> fitnessHistory)
        {
            this.Mutations = mutations;
            this.Crossovers = crossovers;
            this.FitnessEvaluations = fitnessEvaluations;

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
            if (Population != null)
                Population.NoteIndividualMutation();
            Mutations++;
            mutate();
            invalidateCaches();
        }

        public IEvolvable Crossover(IEvolvable mate)
        {
            if (Population != null)
                Population.NoteIndividualCrossover();
            Crossovers++;
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
                ((result as Evolver).Evolvable as EvolvablePopulation).Population = this;

            return result;
        }
    }
}
