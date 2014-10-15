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
                result.Add(new Presentable("Fitness History", fitnessControl()));
                return result;
            }
            set { base.Details = value; }
        }

        private UIElement fitnessControl()
        {
            foreach(var child in historyGrid.Children)
            {
                (child as Rectangle).Fill = null;
                (child as Rectangle).Stroke = null;
            }
            historyGrid.Children.Clear();

            var history = (population as EvolvablePopulation).FitnessHistory;
            int count = Math.Min(history.Count, 100);
            history = history.Take(count).ToList();
            if (count > 0)
            {
                double max = history.Max();
                double min = history.Take(count).Min();
                for (int i = 0; i < count; i++)
                    historyGrid.Children.Add(new Line() { X1 = 10 + 2 * i, Y1 = 80, X2 = 10 + 2 * i, Y2 = 80 - 70 * scale(history.Min(), history.Max(), history[i]), StrokeThickness = 1, Stroke = blackBrush });
            }
            return historyGrid;
        }
        
        private double scale(double min, double max, double v)
        {
            return min == max ? 1.0 : Math.Abs((v - min) / (min - max));
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
