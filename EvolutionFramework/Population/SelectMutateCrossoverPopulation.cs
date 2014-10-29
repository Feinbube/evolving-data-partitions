using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQExtensions;

namespace EvolutionFramework
{
    public class SelectMutateCrossoverPopulation : EvolvablePopulation, IFeedable, IEvolvable, ICreator
    {
        public double MutationRate { get; set; }
        public double EliteClonePercentage { get; set; }
        public double SelectedPercentage { get; set; }
        public double NewPopulationSize { get; set; }

        public int BreedingsSoFar { get; private set; }
        public double Resources { get; private set; }
        public int Feedings { get; private set; }
        public double EnoughFeedingsForBreeding { get; set; }

        public SelectMutateCrossoverPopulation(IPopulation population, Random random, ICreator creator, int populationSize) : base(population, random, creator, populationSize) { construct(0.20, 0.05, 0.30, populationSize, 0, 0, 0, 1); }

        public SelectMutateCrossoverPopulation(IPopulation population, Random random, ICreator creator, int populationSize, double mutationRate, double eliteClonePercentage, double selectedPercentage, double enoughFeedingsForBreeding) : base(population, random, creator, populationSize) { construct(mutationRate, eliteClonePercentage, selectedPercentage, populationSize, 0, 0, 0, enoughFeedingsForBreeding); }

        protected SelectMutateCrossoverPopulation(IPopulation population, double mutationRate, double eliteClonePercentage, double selectedPercentage, double newPopulationSize, int breedingsSoFar, double resources, int feedings, double enoughFeedingsForBreeding, Random random, ICreator creator, int evolvableMutations, int evolvableCrossovers, int evolvableFitnessEvaluations, List<double> fitnessHistory, List<IEvolvable> individuals, IEvolvable bestOfAllTime, int populationSize, long generations, long mutations, long crossovers, long fitnessEvaluations, double foodConsumedInLifetime) : base(population, random, creator, evolvableMutations, evolvableCrossovers, evolvableFitnessEvaluations, fitnessHistory, individuals, bestOfAllTime, populationSize, generations, mutations, crossovers, fitnessEvaluations, foodConsumedInLifetime) { construct(mutationRate, eliteClonePercentage, selectedPercentage, newPopulationSize, breedingsSoFar, resources, feedings, enoughFeedingsForBreeding); }

        protected SelectMutateCrossoverPopulation(SelectMutateCrossoverPopulation original) : base(original) { construct(original.MutationRate, original.EliteClonePercentage, original.SelectedPercentage, original.NewPopulationSize, original.BreedingsSoFar, original.Resources, original.Feedings, original.EnoughFeedingsForBreeding); }

        private void construct(double mutationRate, double eliteClonePercentage, double selectedPercentage, double newPopulationSize, int breedingsSoFar, double resources, int feedings, double enoughFeedingsForBreeding)
        {
            this.MutationRate = mutationRate;
            this.EliteClonePercentage = eliteClonePercentage;
            this.SelectedPercentage = selectedPercentage;
            this.NewPopulationSize = newPopulationSize;

            this.BreedingsSoFar = breedingsSoFar;
            this.Resources = resources;
            this.Feedings = feedings;
            this.EnoughFeedingsForBreeding = enoughFeedingsForBreeding;
        }

        protected override void mutate()
        {
            MutationRate = mutate(MutationRate, 0.01, 1, 1);
            EliteClonePercentage = mutate(EliteClonePercentage, 0.01, 0.3, 1);
            SelectedPercentage = mutate(SelectedPercentage, 0.1, 0.5, 1);
            NewPopulationSize = mutate(NewPopulationSize, 10, 1000, 10);
            EnoughFeedingsForBreeding = mutate(EnoughFeedingsForBreeding, 1, 100000, 10);
        }

        protected override IEvolvable crossover(IEvolvable other)
        {
            if (other is SelectMutateCrossoverPopulation)
            {
                SelectMutateCrossoverPopulation mate = (SelectMutateCrossoverPopulation)other;
                return new SelectMutateCrossoverPopulation(
                        this.ParentPopulation,
                        Evolution.RandomInterpolation(random, this.MutationRate, mate.MutationRate),
                        Evolution.RandomInterpolation(random, this.EliteClonePercentage, mate.EliteClonePercentage),
                        Evolution.RandomInterpolation(random, this.SelectedPercentage, mate.SelectedPercentage),
                        Evolution.RandomInterpolation(random, this.NewPopulationSize, mate.NewPopulationSize),
                        0,
                        (int)Evolution.RandomInterpolation(random, this.Resources, mate.Resources),
                        (int)Evolution.RandomInterpolation(random, this.Feedings, mate.Feedings),
                        Evolution.RandomInterpolation(random, this.EnoughFeedingsForBreeding, mate.EnoughFeedingsForBreeding),
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

        protected override void feed(double resources)
        {
            Resources += resources;

            bool reorder = Resources >= PopulationSize;

            while (Resources >= PopulationSize)
            {
                for (int i = 0; i < PopulationSize; i++)
                {
                    (individuals[i] as IFeedable).Feed(1);
                    Resources -= 1;
                }

                Feedings += 1;

                if (Feedings >= (int)EnoughFeedingsForBreeding)
                {
                    breed();
                    BreedingsSoFar += 1;
                    Feedings -= (int)EnoughFeedingsForBreeding;
                }
            }

            invalidateCaches();
        }

        protected virtual void breed()
        {
            IEvolvable[] newPopulation = new IEvolvable[(int)NewPopulationSize];
            int newPopulationIndex = 0;

            measureFitness();

            var orderedPopulation = individuals.OrderByDescending(a => a.Fitness).ToArray();

            // clone the elite
            for (; newPopulationIndex < (int)Math.Min(PopulationSize, NewPopulationSize * EliteClonePercentage); newPopulationIndex++)
                newPopulation[newPopulationIndex] = orderedPopulation[newPopulationIndex].Clone();

            // mutate some
            for (int i = 0; i < PopulationSize; i++)
                if (random.NextDouble() <= MutationRate)
                    individuals[i].Mutate();

            orderedPopulation = individuals.OrderByDescending(a => a.Fitness).ToArray();

            // select the best
            for (int oldPopulationIndex = 0; newPopulationIndex < (int)Math.Min(PopulationSize, NewPopulationSize * SelectedPercentage); newPopulationIndex++, oldPopulationIndex++)
                set(newPopulation, newPopulationIndex, orderedPopulation[oldPopulationIndex]);

            if (newPopulationIndex <= 1)
                for (; newPopulationIndex < (int)NewPopulationSize; newPopulationIndex++)
                    set(newPopulation, newPopulationIndex, orderedPopulation[0].Clone());
            else
            {
                int parents = newPopulationIndex;
                // crossover to repopulate
                for (; newPopulationIndex < (int)NewPopulationSize; newPopulationIndex++)
                {
                    int parentIndex1 = random.Next(0, parents);
                    int parentIndex2 = random.Next(0, parents - 1);
                    if (parentIndex1 <= parentIndex2) parentIndex2++;
                    set(newPopulation, newPopulationIndex, newPopulation[parentIndex1].Crossover(newPopulation[parentIndex2]));
                }
            }

            this.individuals = newPopulation.OrderByDescending(a => a.Fitness).ToList();

            Generations++;

            invalidateCaches();
        }

        private void set(IEvolvable[] population, int index, IEvolvable value)
        {
            while (contains(population, index, value))
                value.Mutate();
            population[index] = value;
        }

        private bool contains(IEvolvable[] population, int index, IEvolvable value)
        {
            for (int i = 0; i < index; i++)
                if (population[i].Equals(value))
                    return true;
            return false;
        }

        public override string ToString()
        {
            return "Population with " + PopulationSize + " specimen. \r\nFitness: " + Fitness.ToString("F") + " \r\nM:" + MutationRate.ToString("F") + " E:" + EliteClonePercentage.ToString("F") + " S:" + SelectedPercentage.ToString("F") + " F:" + EnoughFeedingsForBreeding.ToString("F"); // Best: " + Best.ToString();
        }

        public override IEvolvable Clone() { return new SelectMutateCrossoverPopulation(this); }

        public override double DifferenceTo(IEvolvable other)
        {
            throw new NotImplementedException();
        }
    }
}
