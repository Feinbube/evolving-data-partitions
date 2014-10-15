using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using ExecutionEnvironment;

namespace EvolutionWpfControls
{
    public class EvolvablePopulationControl : PopulationControl
    {
        static SolidColorBrush blackBrush = (SolidColorBrush)new SolidColorBrush(Colors.Black).GetAsFrozen();
        Grid historyGrid = new Grid();

        public override ObservableCollection<IPresentable> Details
        {
            get
            {
                if (population == null)
                    return null;

                var result = base.Details;
                result.Add(new Presentable("Fitness History", new DoubleArray1DControl()
                {
                    Array = new Arr<double>((population as EvolvablePopulation).FitnessHistory.Take(Math.Min((population as EvolvablePopulation).FitnessHistory.Count, 100)).ToArray())
                }));
                return result;
            }
            set { base.Details = value; }
        }

        protected override IPresentable AsChildPresentable(string title, object evolvable)
        {
            if (evolvable is EvolvablePopulation)
                return new Presentable(title, new EvolvablePopulationControl() { Population = evolvable as EvolvablePopulation });

            return base.AsChildPresentable(title, evolvable);
        }

        protected override List<NamedValue> Stats()
        {
            List<NamedValue> result = base.Stats();
            result.Add(new NamedValue("/", (Population as EvolvablePopulation).EvolveMutations));
            result.Add(new NamedValue("/", (Population as EvolvablePopulation).EvolveCrossovers));
            result.Add(new NamedValue("/", (Population as EvolvablePopulation).EvolveFitnessEvaluations));
            return result;
        }
    }
}
