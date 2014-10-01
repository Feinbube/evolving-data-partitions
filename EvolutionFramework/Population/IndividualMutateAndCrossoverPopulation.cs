using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvolutionFramework
{
    public class IndividualMutateAndCrossoverPopulation : EvolvablePopulation, IFeedable, IEvolvable, ICreator
    {
        protected double maxSize = 0;

        protected List<IEvolvable> newBorns = new List<IEvolvable>();

        public double FoodForPopulation { get; set; }

        public IndividualMutateAndCrossoverPopulation(Random random, ICreator creator, double maxSize) : base(random, creator, (int)maxSize) { this.maxSize = maxSize; }

        public IndividualMutateAndCrossoverPopulation(Random random, ICreator creator, List<IEvolvable> population) : base(random, creator, population) { this.maxSize = population.Count; }

        protected IndividualMutateAndCrossoverPopulation(double maxSize, List<IEvolvable> newBorns, double foodForPopulation, Random random, ICreator creator, int mutations, int crossovers, int fitnessEvaluations, List<double> fitnessHistory, List<IEvolvable> individuals, int populationSize, long generations, long individualMutations, long individualCrossovers, long individualFitnessEvaluations, double foodConsumedInLifetime) : base(random, creator, mutations, crossovers, fitnessEvaluations, fitnessHistory, individuals, populationSize, generations, individualMutations, individualCrossovers, individualFitnessEvaluations, foodConsumedInLifetime) { construct(maxSize, newBorns, foodForPopulation); }

        protected IndividualMutateAndCrossoverPopulation(IndividualMutateAndCrossoverPopulation original) : base(original) { construct(original.maxSize, original.newBorns, original.FoodForPopulation); }

        private void construct(double maxSize, List<IEvolvable> newBorns, double foodForPopulation)
        {
            this.maxSize = maxSize;
            this.newBorns = newBorns.Select(a => a.Clone()).ToList();
            this.FoodForPopulation = foodForPopulation;
        }

        public override void NoteIndividualMutation() { base.NoteIndividualMutation(); invalidateCaches(); }

        public IEvolvable FindMate(IEvolvable evolvable)
        {
            var potentialMates = individuals.OrderByDescending(a => a.Fitness).Take(10);
            return potentialMates.Where(a => evolvable.DifferenceTo(a) == potentialMates.Max(b => evolvable.DifferenceTo(b))).First();
        }

        protected override void feed(double resources)
        {
            FoodForPopulation += resources;
            while (FoodForPopulation >= individuals.Count)
            {
                foreach (IFeedable individual in individuals)
                    individual.Feed(1);

                FoodForPopulation -= individuals.Count;
                nextGeneration();
            }

            sortIndividualsByFitness();
        }

        private void nextGeneration()
        {
            foreach (IEvolvable child in newBorns)
                individuals.Add(child);

            if (newBorns.Count > 0)
                invalidateCaches();

            while (individuals.Count > maxSize)
            {
                individuals.Remove(WorstEvolver);
                invalidateCaches();
            }

            Generations++;
        }

        public void Reproduce(IEvolvable evolvable)
        {
            IEvolvable mate = FindMate(evolvable);
            IEvolvable child = evolvable.Crossover(mate);
            newBorns.Add(child);
        }

        public void Populate()
        {
            base.Populate((int)maxSize);
        }

        public override string ToString()
        {
            return base.ToString() + " Food: " + FoodConsumedInLifetime;
        }

        public override IEvolvable Create() { return new FedEvolver(this, random) { Evolvable = Creator.Create() }; }

        protected override void mutate()
        {
            maxSize = mutate(maxSize, 100, 1000, 10);
        }

        protected override IEvolvable crossover(IEvolvable other)
        {
            IndividualMutateAndCrossoverPopulation mate = (IndividualMutateAndCrossoverPopulation)other;
            return new IndividualMutateAndCrossoverPopulation(
                Evolution.RandomInterpolation(random, this.maxSize, mate.maxSize),
                new List<IEvolvable>(),
                (int)Evolution.RandomInterpolation(random, this.FoodForPopulation, mate.FoodForPopulation),
                random,
                random.NextDouble() < 0.5 ? this.Creator : mate.Creator,
                0, 0, 0,
                null,
                this.individuals.Union(mate.individuals).OrderByDescending(a => a.Fitness).Take((int)Evolution.RandomInterpolation(random, this.PopulationSize, mate.PopulationSize)).ToList(),
                0, 0, 0, 0, 0, 0
                );
        }

        public override IEvolvable Clone()
        {
            return new IndividualMutateAndCrossoverPopulation(this);
        }

        public override double DifferenceTo(IEvolvable other)
        {
            throw new NotImplementedException();
        }
    }
}
