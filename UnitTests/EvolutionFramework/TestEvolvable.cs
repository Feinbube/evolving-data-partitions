using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestEvolvable : IEvolvable
    {
        Random random;
        double[] chromosomes;

        public TestEvolvable(Random random, int length)
        {
            this.random = random;

            chromosomes = new double[length];
            for (int i = 0; i < length; i++)
                chromosomes[i] = random.NextDouble();
        }

        public double Fitness { get { return -chromosomes.Sum(); } }

        public void Mutate()
        {
            chromosomes[random.Next(chromosomes.Length)] += random.NextDouble() - 0.5;
            hashCodeCacheIsInvalid = true;
        }

        public IEvolvable Crossover(IEvolvable other)
        {
            TestEvolvable result = new TestEvolvable(random, chromosomes.Length);
            for (int i = 0; i < chromosomes.Length; i++)
                result.chromosomes[i] = random.NextDouble() < 0.5 ? this.chromosomes[i] : (other as TestEvolvable).chromosomes[i];
            return result;
        }

        public IEvolvable Clone()
        {
            TestEvolvable result = new TestEvolvable(random, chromosomes.Length);
            for (int i = 0; i < chromosomes.Length; i++)
                result.chromosomes[i] = this.chromosomes[i];
            return result;
        }

        public double DifferenceTo(IEvolvable other)
        {
            double result = 0;
            for (int i = 0; i < chromosomes.Length; i++)
                result += Math.Abs(this.chromosomes[i] - (other as TestEvolvable).chromosomes[i]);
            return result;
        }

        public override bool Equals(object obj)
        {
            if (!this.GetType().Equals(obj.GetType()))
                return false;

            if (base.Equals(obj))
                return true;

            if (this.GetHashCode() != obj.GetHashCode())
                return false;

            TestEvolvable other = (TestEvolvable)obj;
            if (this.chromosomes.Length != other.chromosomes.Length) return false;
            for (int i = 0; i < this.chromosomes.Length; i++)
                if (chromosomes[i] != other.chromosomes[i])
                    return false;

            return true;
        }

        private bool hashCodeCacheIsInvalid = true;
        private int hashCodeCache = 0;

        public override int GetHashCode()
        {
            if (hashCodeCacheIsInvalid)
            {
                hashCodeCache = this.chromosomes.Length;
                for (int i = 0; i < this.chromosomes.Length; i++)
                    hashCodeCache += this.chromosomes[i].GetHashCode();
                hashCodeCacheIsInvalid = false;
            }
            return hashCodeCache;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        public bool IsValid
        {
            get { return true; }
        }


        public void Leap()
        {
            throw new NotImplementedException();
        }
    }
}
