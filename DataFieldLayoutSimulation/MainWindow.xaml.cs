using DrawingSupport;
using EvolutionFramework;
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
        Random random = new Random(2014);
        bool running = true;

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
            ICreator creator = new StencilSpeciesArrCreator(random, 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 });
            evolution = new Evolution(random, creator, 7, 64) { };
            //evolution = new Evolution(new StencilSpeciesCreator(new Random(), 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 }), 5, 100) { };
            //evolution = new Evolution(new StencilSpeciesCreator(new Random(), 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 }), 7, 25) { };
            //evolution = new Evolution(new TestSpeciesCreator(), 7, 25) { };

            evolutionForView = (Evolution)evolution.Clone();

            while (running)
            {
                evolution.Feed(10000);
                evolutionForView = (Evolution)evolution.Clone();
                //Thread.Sleep(1000);
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
    }
}
