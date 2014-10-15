using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class Evolver : Evolvable, IEvolver, IFeedable
    {
        public IEvolvable Evolvable { get; set; }

        public Evolver(IPopulation population) : base(population) { }

        public Evolver(IPopulation population, IEvolvable evolvable) : base(population) { Evolvable = evolvable; }

        protected override double assessFitness()
        {
            return Evolvable.Fitness;
        }

        protected override void mutate()
        {
            Evolvable.Mutate();
        }

        protected override IEvolvable crossover(IEvolvable mate)
        {
            return new Evolver(population, mate is Evolver ? Evolvable.Crossover((mate as Evolver).Evolvable) : Evolvable.Crossover(mate));
        }

        protected override double differenceTo(IEvolvable other)
        {
            return other is Evolver ? Evolvable.DifferenceTo((other as Evolver).Evolvable) : Evolvable.DifferenceTo(other);
        }

        protected override IEvolvable clone()
        {
            return new Evolver(population, Evolvable.Clone());
        }

        public double FoodConsumedInLifetime
        {
            get { return (Evolvable is IFeedable) ? (Evolvable as IFeedable).FoodConsumedInLifetime : 0; }
        }

        public void Feed(double resources)
        {
            if(Evolvable is IFeedable) 
                (Evolvable as IFeedable).Feed(resources);
        }

        public override long Mutations { get { return Evolvable is IPopulation ? (Evolvable as IPopulation).Mutations : base.Mutations; } }
        public override long Crossovers { get { return Evolvable is IPopulation ? (Evolvable as IPopulation).Crossovers : base.Crossovers; } }
        public override long FitnessEvaluations { get { return Evolvable is IPopulation ? (Evolvable as IPopulation).FitnessEvaluations : base.FitnessEvaluations; } }

        public override bool Equals(object obj)
        {
            return Evolvable.Equals((obj as Evolver).Evolvable);
        }

        public override int GetHashCode()
        {
            return Evolvable.GetHashCode();
        }
    }
}
