using Algorithms;
using ExecutionEnvironment;
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

namespace AlgorithmPlotter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Plotter plotter = new ReadWritePlotter();
        Algorithm algorithm = new MatrixVectorMultiplication();

        public List<Algorithm> Algorithms { get { return Registry.RegisteredAlgorithms.ToList(); } }
        public List<Plotter> Plotters { get { return new List<Plotter>() { new ReadWriteDensePlotter(), new ReadWritePlotter(), new RowPerRoundPlotter(), new TwoDPlotter() }; } }

        public MainWindow()
        {
            InitializeComponent();
            window.DataContext = this;

            Reset();
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            uiScaleSliderX.Value *= (e.Delta > 0) ? 1.1 : 0.9;
            uiScaleSliderY.Value *= (e.Delta > 0) ? 1.1 : 0.9;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                var position = e.GetPosition(this);
                uiTranslateSliderX.Value += (position.X - mousePos.X) / uiScaleSliderX.Value;
                uiTranslateSliderY.Value += (position.Y - mousePos.Y) / uiScaleSliderY.Value;
                mousePos = position;
            }
        }

        bool moving = false;
        Point mousePos;

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            moving = true;
            mousePos = e.GetPosition(this);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            moving = false;
        }

        private void ComboBoxAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            algorithm = (Algorithm)e.AddedItems[0];
            Reset();
        }

        private void ComboBoxPlotter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            plotter = (Plotter)e.AddedItems[0];
            Reset();
        }

        private void Reset()
        {
            plotter.Surface = grid;
            Memory.Instance.Plotter = plotter;

            plotter.Reset();
            
            Memory.Instance.Clear();
            algorithm.Run(5, 5, 4, false, 1, 0);
        }
    }
}
