using EvolutionFramework;
using System;
using System.Collections.Generic;
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
    public partial class BreedingPoolControl : UserControl
    {
        public BreedingPool Pool { get; set; }

        public bool ShowPopulation { get; set; }

        static SolidColorBrush blackBrush = (SolidColorBrush)new SolidColorBrush(Colors.Black).GetAsFrozen();

        public BreedingPool BreedingPool
        {
            get { return Pool; }
            set
            {
                Pool = value;

                this.DataContext = null;
                this.DataContext = this;

                historyGrid.Children.Clear();
                if (value != null && value is BreedingPool)
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

                populationList.Items.Clear();
                if (ShowPopulation && value != null)
                    for (int i = 0; i < Pool.Population.Length; i++)
                    {
                        IEvolvable evolvable = Pool.Population[i];
                        if (evolvable is BreedingPool)
                            populationList.Items.Add(new BreedingPoolControl() { ShowPopulation = true /*i == 0*/, BreedingPool = (BreedingPool)evolvable });
                        else if (evolvable is IPresentable)
                            populationList.Items.Add(((IPresentable)evolvable).AsControl());
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
    }
}
