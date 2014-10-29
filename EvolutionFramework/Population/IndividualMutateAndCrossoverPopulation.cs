using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQExtensions;

namespace EvolutionFramework
{
    public class IndividualMutateAndCrossoverPopulation : EvolvablePopulation, IFeedable, IEvolvable, ICreator
    {
        public double MaxSize { get; protected set; }

        protected List<IEvolvable> newBorns = new List<IEvolvable>();

        public double FoodForPopulation { get; set; }

        public IndividualMutateAndCrossoverPopulation(IPopulation population, Random random, ICreator creator, double maxSize) : base(population, random, creator, (int)maxSize) { this.MaxSize = maxSize; }

        public IndividualMutateAndCrossoverPopulation(IPopulation population, Random random, ICreator creator, List<IEvolvable> individuals) : base(population, random, creator, individuals) { this.MaxSize = individuals.Count; }

        protected IndividualMutateAndCrossoverPopulation(IPopulation population, double maxSize, List<IEvolvable> newBorns, double foodForPopulation, Random random, ICreator creator, int evolvableMutations, int evolvableCrossovers, int evolvableFitnessEvaluations, List<double> fitnessHistory, List<IEvolvable> individuals, IEvolvable bestOfAllTime, int populationSize, long generations, long mutations, long crossovers, long fitnessEvaluations, double foodConsumedInLifetime) : base(population, random, creator, evolvableMutations, evolvableCrossovers, evolvableFitnessEvaluations, fitnessHistory, individuals, bestOfAllTime, populationSize, generations, mutations, crossovers, fitnessEvaluations, foodConsumedInLifetime) { construct(maxSize, newBorns, foodForPopulation); }

        protected IndividualMutateAndCrossoverPopulation(IndividualMutateAndCrossoverPopulation original) : base(original) { construct(original.MaxSize, original.newBorns, original.FoodForPopulation); }

        private void construct(double maxSize, List<IEvolvable> newBorns, double foodForPopulation)
        {
            this.MaxSize = maxSize;
            this.newBorns = newBorns.Select(a => a.Clone()).ToList();
            this.FoodForPopulation = foodForPopulation;
        }

        public IEvolvable FindMate(IEvolvable evolvable)
        {
            // return IndividualsSortedByFitness.Take(10).MaxElement(a => evolvable.DifferenceTo(a));

            // sort by difference and then take the best out of the top 10! :D
            return Individuals.OrderByDescending(a => evolvable.DifferenceTo(a)).Take(10).OrderByDescending(a => a.Fitness).First();

            //return IndividualsSortedByFitness.Take(3).MaxElement(a => evolvable.DifferenceTo(a));
            //return Best;
            //return IndividualsSortedByFitness.MaxElement(a => evolvable.DifferenceTo(a));
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
                invalidateCaches();
                measureFitness();
            }
        }

        private void nextGeneration()
        {
            foreach (IEvolvable child in newBorns)
            {
                while (contains(individuals, child))  // individuals.Contains(child))
                    child.Mutate();
                individuals.Add(child);
            }

            if (newBorns.Count > 0)
                invalidateCaches();
            newBorns.Clear();

            while (individuals.Count > MaxSize)
            {
                individuals.Remove(WorstEvolver);
                invalidateCaches();
            }

            Generations++;
        }

        private bool contains(List<IEvolvable> list, IEvolvable value)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].Equals(value))
                    return true;
            return false;
        }

        public void AddChild(IEvolvable child)
        {
            newBorns.Add(child);
        }

        public void Populate()
        {
            base.Populate((int)MaxSize);
        }

        public override string ToString()
        {
            return "Population with " + PopulationSize + " specimen. \r\nFitness: " + Fitness.ToString("F");
        }

        public override IEvolvable Create() { return new FedEvolver(this, random) { Evolvable = Creator.Create() }; }

        protected override void mutate()
        {
            MaxSize = mutate(MaxSize, 100, 1000, 10);
        }

        protected override IEvolvable crossover(IEvolvable other)
        {
            if (other is IndividualMutateAndCrossoverPopulation)
            {
                IndividualMutateAndCrossoverPopulation mate = (IndividualMutateAndCrossoverPopulation)other;
                return new IndividualMutateAndCrossoverPopulation(
                    this.ParentPopulation,
                    Evolution.RandomInterpolation(random, this.MaxSize, mate.MaxSize),
                    new List<IEvolvable>(),
                    (int)Evolution.RandomInterpolation(random, this.FoodForPopulation, mate.FoodForPopulation),
                    random,
                    random.NextDouble() < 0.5 ? this.Creator : mate.Creator,
                    0, 0, 0,
                    this.Fitness > mate.Fitness ? this.FitnessHistory.Select(a => a).ToList() : mate.FitnessHistory.Select(a => a).ToList(),
                    this.individuals.Union(mate.individuals).OrderByDescending(a => a.Fitness).Take((int)Evolution.RandomInterpolation(random, this.PopulationSize, mate.PopulationSize)).ToList(),
                    this.BestOfAllTime.Fitness > mate.BestOfAllTime.Fitness ? this.BestOfAllTime : mate.BestOfAllTime,
                    0, 0, Mutations + mate.Mutations, Crossovers + mate.Crossovers, FitnessEvaluations + mate.FitnessEvaluations, 0
                    );
            }
            else return base.crossover(other);
        }

        public override IEvolvable Clone()
        {
            return new IndividualMutateAndCrossoverPopulation(this);
        }

        public override double DifferenceTo(IEvolvable other)
        {
            return Math.Abs(MaxSize - (other as IndividualMutateAndCrossoverPopulation).MaxSize);
        }
    }
}
