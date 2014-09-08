using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class BreedingPool : IEvolvable
    {
        public ICreator Creator = null;
        public IEvolvable[] Population = null;

        public double MutationRate { get; set; }
        public double EliteClonePercentage { get; set; }
        public double SelectedPercentage { get; set; }
        public double NewPopulationSize { get; set; }

        public long ResourcesFedSoFar { get; private set; }
        public int BreedingsSoFar { get; private set; }
        public int Resources { get; private set; }
        public int Feedings { get; private set; }
        public double EnoughFeedingsForBreeding { get; set; }

        public int PopulationSize { get { return Population == null ? 0 : Population.Length; } }

        private double fitness = double.NaN;
        public double Fitness { get { if (fitness.Equals(double.NaN)) fitness = Population.Max(a => a.Fitness); return fitness; } }

        private List<double> fitnessHistory = new List<double>();
        public List<double> FitnessHistory { get { lock (fitnessHistory) { return fitnessHistory.Select(a => a).ToList(); } } }

        public IEvolvable Best { get { return Population.First(a => a.Fitness >= Fitness); } }

        public int Generation { get; set; }

        public BreedingPool(ICreator creator, int populationSize) : this(creator, populationSize, 0.20, 0.05, 0.30, 1) { }

        public BreedingPool(ICreator creator, int populationSize, double mutationRate, double eliteClonePercentage, double selectedPercentage, double enoughFeedingsForBreeding)
            : this(creator, mutationRate, eliteClonePercentage, selectedPercentage, populationSize, enoughFeedingsForBreeding, 0, 0, 0, 0, null, double.NaN, 0, null) { }

        public BreedingPool(BreedingPool original)
            : this(original.Creator, original.MutationRate, original.EliteClonePercentage, original.SelectedPercentage, original.NewPopulationSize,
            original.EnoughFeedingsForBreeding, original.ResourcesFedSoFar, original.BreedingsSoFar, original.Resources, original.Feedings,
            original.Population, original.fitness, original.Generation, original.fitnessHistory.Select(a => a).ToList()) { }

        public BreedingPool(ICreator creator,
            double mutationRate, double eliteClonePercentage, double selectedPercentage, double newPopulationSize,
            double enoughFeedingsForBreeding, long resourcesFedSoFar, int breedingsSoFar, int resources, int feedings,
            IEvolvable[] population, double fitness, int generation, List<double> fitnessHistory)
        {
            this.Creator = creator;
            this.MutationRate = mutationRate;
            this.EliteClonePercentage = eliteClonePercentage;
            this.SelectedPercentage = selectedPercentage;
            this.NewPopulationSize = newPopulationSize;
            this.EnoughFeedingsForBreeding = enoughFeedingsForBreeding;
            this.ResourcesFedSoFar = resourcesFedSoFar;
            this.BreedingsSoFar = breedingsSoFar;
            this.Resources = resources;
            this.Feedings = feedings;


            if (population != null)
            {
                this.Population = new IEvolvable[population.Length];
                for (int i = 0; i < population.Length; i++)
                    this.Population[i] = population[i].Clone();
            }
            else
            {
                this.Population = new IEvolvable[(int)newPopulationSize];
                for (int i = 0; i < (int)newPopulationSize; i++)
                    this.Population[i] = Creator.Create();
            }
            this.Generation = generation;
            if (fitnessHistory != null)
                this.fitnessHistory = fitnessHistory;
        }

        public IEvolvable Clone() { return new BreedingPool(this); }

        public void Mutate(Random random)
        {
            MutationRate = mutate(random, MutationRate, 0.01, 1, 1);
            EliteClonePercentage = mutate(random, EliteClonePercentage, 0.01, 0.3, 1);
            SelectedPercentage = mutate(random, SelectedPercentage, 0.1, 0.5, 1);
            NewPopulationSize = mutate(random, NewPopulationSize, 100, 1000, 10);
            EnoughFeedingsForBreeding = mutate(random, EnoughFeedingsForBreeding, 1, 100000, 10);
        }

        private double mutate(Random random, double value, double min, double max, double scale)
        {
            return Math.Min(max, Math.Max(min, value + (random.NextDouble() - 0.5) * scale));
        }

        public IEvolvable Crossover(Random random, IEvolvable other)
        {
            BreedingPool mate = (BreedingPool)other;
            return new BreedingPool(
                    random.NextDouble() < 0.5 ? this.Creator : mate.Creator,
                    Evolution.RandomInterpolation(random, this.MutationRate, mate.MutationRate),
                    Evolution.RandomInterpolation(random, this.EliteClonePercentage, mate.EliteClonePercentage),
                    Evolution.RandomInterpolation(random, this.SelectedPercentage, mate.SelectedPercentage),
                    Evolution.RandomInterpolation(random, this.NewPopulationSize, mate.NewPopulationSize),
                    Evolution.RandomInterpolation(random, this.EnoughFeedingsForBreeding, mate.EnoughFeedingsForBreeding),
                    0,
                    0,
                    (int)Evolution.RandomInterpolation(random, this.Resources, mate.Resources),
                    (int)Evolution.RandomInterpolation(random, this.Feedings, mate.Feedings),
                    Population.Union(mate.Population).OrderByDescending(a => a.Fitness).Take((int)Evolution.RandomInterpolation(random, this.PopulationSize, mate.PopulationSize)).ToArray(),
                    double.NaN, 0, null
                );
        }

        public virtual void Feed(Random random, int resources)
        {
            ResourcesFedSoFar += resources;
            Resources += resources;

            while (Resources >= PopulationSize)
            {
                for (int i = 0; i < PopulationSize; i++)
                {
                    Population[i].Feed(random, 1);
                    Resources -= 1;
                }

                Feedings += 1;

                if (Feedings >= (int)EnoughFeedingsForBreeding)
                {
                    breed(random);
                    BreedingsSoFar += 1;
                    Feedings -= (int)EnoughFeedingsForBreeding;
                }
            }

            resetFitness();
        }

        protected virtual void breed(Random random)
        {
            IEvolvable[] newPopulation = new IEvolvable[(int)NewPopulationSize];
            int newPopulationIndex = 0;

            lock (fitnessHistory)
            {
                fitnessHistory.Insert(0, Fitness);
            }

            var orderedPopulation = Population.OrderByDescending(a => a.Fitness).ToArray();

            // clone the elite
            for (; newPopulationIndex < (int)Math.Min(PopulationSize, NewPopulationSize * EliteClonePercentage); newPopulationIndex++)      
                newPopulation[newPopulationIndex] = orderedPopulation[newPopulationIndex].Clone();
      
            // mutate some
            for (int i = 0; i < PopulationSize; i++)
                if (random.NextDouble() <= MutationRate)
                    Population[i].Mutate(random);

            orderedPopulation = Population.OrderByDescending(a => a.Fitness).ToArray();

            // select the best
            for (int oldPopulationIndex = 0; newPopulationIndex < (int)Math.Min(PopulationSize, NewPopulationSize * SelectedPercentage); newPopulationIndex++, oldPopulationIndex++)
                set(random, newPopulation, newPopulationIndex, orderedPopulation[oldPopulationIndex]);

            if (newPopulationIndex <= 1)
                for (; newPopulationIndex < (int)NewPopulationSize; newPopulationIndex++)
                    set(random, newPopulation, newPopulationIndex, orderedPopulation[0].Clone());
            else
            {
                int parents = newPopulationIndex;
                // crossover to repopulate
                for (; newPopulationIndex < (int)NewPopulationSize; newPopulationIndex++)
                {
                    int parentIndex1 = random.Next(0, parents);
                    int parentIndex2 = random.Next(0, parents - 1);
                    if (parentIndex1 <= parentIndex2) parentIndex2++;
                    set( random, newPopulation, newPopulationIndex, newPopulation[parentIndex1].Crossover(random, newPopulation[parentIndex2]));
                }
            }

            this.Population = newPopulation.OrderByDescending(a => a.Fitness).ToArray();
            
            Generation++;

            resetFitness();
        }

        private void set(Random random, IEvolvable[] population, int index, IEvolvable value)
        {
            while (contains(population, index, value))
                value.Mutate(random);
            population[index] = value;
        }

        private bool contains(IEvolvable[] population, int index, IEvolvable value)
        {
            for (int i = 0; i < index; i++)
                if (population[i].Equals(value))
                    return true;
            return false;
        }

        protected void resetFitness()
        {
            fitness = double.NaN;
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

        public override string ToString()
        {
            return "Breeding pool with " + PopulationSize + " specimen (" + MutationRate.ToString("F") + "/" + EliteClonePercentage.ToString("F") + "/" + SelectedPercentage.ToString("F") + "/" + EnoughFeedingsForBreeding.ToString("F") + "). Best: " + Best.ToString();
        }
    }
}
