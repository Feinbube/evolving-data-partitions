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

namespace SingleIndividualMutator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool running = true;

        public StencilSpeciesArr Individual { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Individual = (StencilSpeciesArr)new StencilSpeciesArrCreator(new Random(), 100, 100, new double[] { 0.25, 0.25, 0.25, 0.25 }).Create();
            //Individual = (StencilSpeciesArr)new StencilSpeciesArrCreator(new Random(), 10, 10, new double[] { 0.25, 0.25, 0.25, 0.25 }).Create();
            this.Content = Individual.PresentableControl;

            new Thread(new ThreadStart(work)).Start();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 3000), DispatcherPriority.Background, updateView, Dispatcher.CurrentDispatcher);
        }

        void work()
        {
            while (running && Individual.NextStep()) ;
            while (running && Individual.NextStepX()) ;

            if (running)
            {
                running = false;

                while (true)
                    if (Individual.NextStepX())
                        throw new Exception("WTF");
            }
        }

        void updateView(object sender, EventArgs e)
        {
            this.Content = null;
            this.Content = Individual.PresentableControl;

            this.Title = "Running: " + running;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            running = false;
        }
    }
}
