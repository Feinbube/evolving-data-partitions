using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EvolutionWpfControls
{
    /// <summary>
    /// Interaction logic for BreedingPoolControl.xaml
    /// </summary>
    public partial class BreedingPoolControl : UserControl, INotifyPropertyChanged
    {
        public EvolvablePopulation Pool { get; set; }

        public string PoolType { get { return Pool == null ? "null" : Pool.GetType().ToString(); } }

        public bool ShowPopulation { get; set; }

        static SolidColorBrush blackBrush = (SolidColorBrush)new SolidColorBrush(Colors.Black).GetAsFrozen();

        public EvolvablePopulation BreedingPool
        {
            get { return Pool; }
            set
            {
                Pool = value;

                this.DataContext = null;
                this.DataContext = this;

                historyGrid.Children.Clear();
                if (value != null && value is EvolvablePopulation)
                {
                    var history = Pool.FitnessHistory;
                    int count = Math.Min(history.Count, 100);
                    history = history.Take(count).ToList();
                    if (count > 0)
                    {
                        double max = history.Max();
                        double min = history.Take(count).Min();
                        for (int i = 0; i < count; i++)
                            historyGrid.Children.Add(new Line() { X1 = 10 + 2 * i, Y1 = 80, X2 = 10 + 2 * i, Y2 = 80 - 70 * scale(history.Min(), history.Max(), history[i]), StrokeThickness = 1, Stroke = blackBrush });
                    }
                }

                if (Pool == null)
                {
                    groupBoxBest.Content = null;
                    groupBoxWorst.Content = null;
                }
                else
                {
                    groupBoxBest.Content = Pool.Best is IPresentable ? (Pool.Best as IPresentable).PresentableControl : new Label() { Content = Pool.Best.Fitness.ToString() };
                    groupBoxWorst.Content = Pool.Worst is IPresentable ? (Pool.Worst as IPresentable).PresentableControl : new Label() { Content = Pool.Worst.Fitness.ToString() };
                }

                populationList.Items.Clear();
                if (ShowPopulation && value != null)
                    for (int i = 0; i < Pool.Individuals.Count; i++)
                    {
                        IEvolvable evolvable = Pool.Individuals[i];
                        if (evolvable is EvolvablePopulation)
                            populationList.Items.Add(new BreedingPoolControl() { ShowPopulation = true /*i == 0*/, BreedingPool = (EvolvablePopulation)evolvable });
                        else if (evolvable is IPresentable)
                            populationList.Items.Add(((IPresentable)evolvable).PresentableControl);
                        else
                            populationList.Items.Add(evolvable.ToString());
                    }
            }
        }

        public BreedingPoolControl()
        {
            ShowPopulation = false;
            InitializeComponent();
        }

        private double scale(double min, double max, double v)
        {
            return min == max ? 1.0 : Math.Abs((v - min) / (min - max));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
