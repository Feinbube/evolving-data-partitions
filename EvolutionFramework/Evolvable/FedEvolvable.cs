using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public abstract class FedEvolvable : Evolvable, IFeedable
    {
        protected Random random = null;

        public double FoodNeededForReproduction { get; set; }
        public double FoodForReproduction { get; set; }
        public double FoodNeededForMutation { get; set; }
        public double FoodForMutation { get; set; }

        public double FoodConsumedInLifetime { get; set; }

        public FedEvolvable(IndividualMutateAndCrossoverPopulation population, Random random) : base(population)
        {
            this.random = random;
            FoodNeededForMutation = 3;
            FoodNeededForReproduction = 10;
        }

        public void Feed(double resources)
        {
            FoodConsumedInLifetime += resources;
            FoodForMutation += resources;
            FoodForReproduction += resources;  

            if (FoodForMutation >= FoodNeededForMutation)
            {
                Mutate();
                FoodForMutation -= FoodNeededForMutation;

                FoodNeededForMutation = mutate(random, FoodNeededForMutation, 1, 100, 10);
                FoodNeededForReproduction = mutate(random, FoodNeededForReproduction, 1, 100, 10);
            }

            if (FoodForReproduction >= FoodNeededForReproduction)
            {
                (population as IndividualMutateAndCrossoverPopulation).Reproduce(this);
                FoodForReproduction -= FoodNeededForReproduction;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " Food needed for Mutation: " + FoodNeededForMutation + " for Reproduction: " + FoodNeededForReproduction + " consumed: " + FoodConsumedInLifetime;
        }
    }
}
