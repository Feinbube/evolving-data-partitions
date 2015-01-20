using DrawingSupport;
using EvolutionFramework;
using Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace DataFieldLayoutSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool running = true;
        bool leapNow = false;

        Evolution evolution;
        Evolution evolutionForView;

        public MainWindow()
        {
            InitializeComponent();

            new Thread(new ThreadStart(work)).Start();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 3000), DispatcherPriority.Background, updateView, Dispatcher.CurrentDispatcher);
        }

        void work()
        {
            //evolution = new Evolution(new Random(), new StencilSpeciesArrCreator(new Random(), 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 }), 7, 25) { };
            //evolution = new Evolution(new Random(), new StencilSpeciesArrCreator(new Random(), 50, 50, new double[] { 0.25, 0.25, 0.25, 0.25 }), 1, 5) { };
            evolution = new Evolution(new Random(), new StencilSpeciesArrCreator(new Random(), 10, 10, 4), 7, 25) { };

            evolutionForView = (Evolution)evolution.Clone();

            while (running)
            {
                evolution.Feed(42);
                evolutionForView = (Evolution)evolution.Clone();
                //Thread.Sleep(1000);

                if(leapNow)
                {
                    evolution.Leap();
                    leapNow = false;
                }
            }
        }

        void updateView(object sender, EventArgs e)
        {
            //this.evolutionControl.Evolution = null;
            //this.evolutionControl.Evolution = evolutionForView;

            this.evolutionControlNew.Population = evolutionForView;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            running = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            leapNow = true;
        }
    }
}
