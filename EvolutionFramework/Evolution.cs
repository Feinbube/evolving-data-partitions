using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class Evolution : BreedingPool
    {
        public bool Pause { get; set; }

        public int SimulationRound { get { return this.Generation; } }
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

        public Evolution(ICreator creator, int breedingPools, int populationPerPool) : base(new BreedingPoolCreator(creator, populationPerPool, 1), breedingPools) { Init(); }
        public Evolution(ICreator creator, int colonies, int breedingPools, int populationPerPool) : base(new BreedingPoolCreator(new BreedingPoolCreator(creator, populationPerPool, 1), breedingPools, 100), colonies) { Init(); this.EnoughFeedingsForBreeding = 1000; }

        public bool Stopped
        {
            get
            {
                return Pause ||
                    PopulationSize == 0 ||
                    Runtime > MaxRuntime ||
                    SimulationRound > MaxSimulationRounds ||
                    Fitness > MaxFitness ||
                    ResourcesFedSoFar > MaxFeedings ||
                    RoundsWithoutFitnessChange > MaxRoundsWithoutFitnessChange;
            }
        }

        public override void Feed(Random random, int resources)
        {
            if (Stopped)
                return;

            DateTime start = DateTime.Now;
            base.Feed(random, resources);
            runtimeInMilliseconds += (DateTime.Now - start).TotalMilliseconds;
        }

        protected override void breed(Random random)
        {
            base.breed(random);

            if (FitnessHistory.Count > 0 && FitnessHistory[0] == Fitness) RoundsWithoutFitnessChange++;
            else RoundsWithoutFitnessChange = 0;
        }

        public static double RandomInterpolation(Random random, double a, double b) { return random.NextDouble() * (b - a) + a; }

        public override string ToString()
        {
            return "[" + Generation + "] Evolution with " + PopulationSize + " breeding pools. Best: " + Best.ToString();
        }        
    }
}
