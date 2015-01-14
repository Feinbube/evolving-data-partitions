using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class Evolution : SelectMutateCrossoverPopulation
    {
        public bool Pause { get; set; }

        public long SimulationRound { get { return this.Generations; } }
        public int MaxSimulationRounds { get; set; } // Fixed number of generations reached

        private double runtimeInMilliseconds = 0;
        public TimeSpan Runtime { get { return new TimeSpan(0, 0, 0, 0, (int)runtimeInMilliseconds); } }
        public TimeSpan MaxRuntime { get; set; } // Allocated budget (computation time/money) reached

        public double MaxFitness { get; set; } // A solution is found that satisfies minimum criteria

        public int RoundsWithoutFitnessChange { get; set; }
        public int MaxRoundsWithoutFitnessChange { get; set; } // The highest ranking solution's fitness is reaching or has reached a plateau such that successive iterations no longer produce better results

        public long MaxFeedings { get; set; }

        private void Init()
        {
            this.Pause = false;
            this.MaxSimulationRounds = int.MaxValue;
            this.MaxRuntime = new TimeSpan(366, 0, 0, 0);
            this.MaxFitness = double.MaxValue;
            this.MaxRoundsWithoutFitnessChange = int.MaxValue;
            this.MaxFeedings = long.MaxValue;
            this.EnoughFeedingsForBreeding = 10000;
        }

        public Evolution(Random random, ICreator creator, int breedingPools, int populationPerPool) : base(null, random, new SelectMutateCrossoverPopulationCreator(null, random, creator, populationPerPool, 1) , breedingPools) { Init(); }

        //public Evolution(Random random, ICreator creator, int breedingPools, int populationPerPool) : base(null, random, new IndividualMutateAndCrossoverPopulationCreator(null, random, creator, populationPerPool), breedingPools) { Init(); }

        public Evolution(Evolution evolution)
            : base(evolution)
        {
            Pause = evolution.Pause;
            MaxSimulationRounds = evolution.MaxSimulationRounds;
            MaxRuntime = evolution.MaxRuntime;
            MaxFitness = evolution.MaxFitness;
            MaxRoundsWithoutFitnessChange = evolution.MaxRoundsWithoutFitnessChange;
            MaxFeedings = evolution.MaxFeedings;
            EnoughFeedingsForBreeding = evolution.EnoughFeedingsForBreeding;
            runtimeInMilliseconds = evolution.runtimeInMilliseconds;
            RoundsWithoutFitnessChange = evolution.RoundsWithoutFitnessChange;
        }

        // public Evolution(Random random, ICreator creator, int colonies, int breedingPools, int populationPerPool) : base(random, new SelectMutateCrossoverPopulationCreator(random, new SelectMutateCrossoverPopulationCreator(random, creator, populationPerPool, 1), breedingPools, 100), colonies) { Init(); this.EnoughFeedingsForBreeding = 1000; }

        public bool Stopped
        {
            get
            {
                return Pause ||
                    PopulationSize == 0 ||
                    Runtime > MaxRuntime ||
                    SimulationRound > MaxSimulationRounds ||
                    Fitness > MaxFitness ||
                    FoodConsumedInLifetime > MaxFeedings ||
                    RoundsWithoutFitnessChange > MaxRoundsWithoutFitnessChange;
            }
        }

        protected override void feed(double resources)
        {
            if (Stopped)
                return;

            DateTime start = DateTime.Now;
            base.feed(resources);
            runtimeInMilliseconds += (DateTime.Now - start).TotalMilliseconds;
        }

        protected override void breed()
        {
            base.breed();

            if (FitnessHistory.Count > 0 && FitnessHistory[0] == Fitness) RoundsWithoutFitnessChange++;
            else RoundsWithoutFitnessChange = 0;
        }

        public static double RandomInterpolation(Random random, double a, double b) { return random.NextDouble() * (b - a) + a; }

        public override string ToString()
        {
            return "[" + Generations + "] Evolution with " + PopulationSize + " breeding pools. Best: " + Best.ToString();
        }

        public override IEvolvable Clone() { return new Evolution(this); }
    }
}
