﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class FedEvolver : FedEvolvable, IEvolver, IFeedable
    {
        public IEvolvable Evolvable { get; set; }

        public FedEvolver(IndividualMutateAndCrossoverPopulation population, Random random) : base(population, random) { }

        public FedEvolver(IndividualMutateAndCrossoverPopulation population, IEvolvable evolvable, Random random) : base(population, random) { this.Evolvable = evolvable; }

        public override double Fitness { get { return Evolvable is IPopulation ? (Evolvable as IPopulation).Fitness : base.Fitness; } }

        protected override double assessFitness() { return Evolvable.Fitness; }

        protected override void mutate()
        {
            Evolvable.Mutate();
        }

        protected override void leap()
        {
            Evolvable.Leap();
        }

        protected override IEvolvable crossover(IEvolvable mate)
        {
            return new FedEvolver((population as IndividualMutateAndCrossoverPopulation), mate is FedEvolver ? Evolvable.Crossover((mate as FedEvolver).Evolvable) : Evolvable.Crossover(mate), random);
        }

        protected override double differenceTo(IEvolvable other)
        {
            return other is FedEvolver ? Evolvable.DifferenceTo((other as FedEvolver).Evolvable) : Evolvable.DifferenceTo(other);
        }

        protected override IEvolvable clone()
        {
            return new FedEvolver((population as IndividualMutateAndCrossoverPopulation), Evolvable.Clone(), random);
        }

        public override long Mutations { get { return Evolvable is IPopulation ? (Evolvable as IPopulation).Mutations : base.Mutations; } }
        public override long Crossovers { get { return Evolvable is IPopulation ? (Evolvable as IPopulation).Crossovers : base.Crossovers; } }
        public override long FitnessEvaluations { get { return Evolvable is IPopulation ? (Evolvable as IPopulation).FitnessEvaluations : base.FitnessEvaluations; } }

        public override bool Equals(object obj)
        {
            return Evolvable.Equals((obj as FedEvolver).Evolvable);
        }

        public override int GetHashCode()
        {
            return Evolvable.GetHashCode();
        }

        public override bool IsValid
        {
            get { return Evolvable.IsValid; }
        }
    }
}
