using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public interface IEvolvable : INotifyPropertyChanged
    {
        double Fitness { get; }

        void Mutate();

        void Leap();
        
        IEvolvable Crossover(IEvolvable other);

        IEvolvable Clone();

        double DifferenceTo(IEvolvable other);

        bool IsValid { get; }
    }
}
