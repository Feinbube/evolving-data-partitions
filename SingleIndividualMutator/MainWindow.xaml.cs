using ExecutionEnvironment;
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
        IterativeSingleIndividualMutator mutator = new IterativeSingleIndividualMutator(16, 16, 4);
        bool running = true;

        public MainWindow()
        {
            InitializeComponent();

            int a = 50;
            StencilSpeciesArr it = new StencilSpeciesArr(new Random(), optimal4Processors(a), equalProcessors(4, a), new Mutator[] { new JumpToSameColorMutator() });
            this.Title = "" + it.IsValid + " " + it.Fitness;
            this.Content = it.PresentableControl;

            mutator.Individual = it;
            this.Content = mutator.Individual.PresentableControl;

            new Thread(new ThreadStart(work)).Start();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 3000), DispatcherPriority.Background, updateView, Dispatcher.CurrentDispatcher);
        }

        void work()
        {
            while (running)
                mutator.Progress();
        }

        void updateView(object sender, EventArgs e)
        {
            this.Content = null;
            this.Content = mutator.Individual.PresentableControl;

            this.Title = "Running: " + running;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            running = false;
        }

        private int[] equalProcessors(int count, int a)
        {
            int[] result = new int[count];
            for (int i = 0; i < count; i++)
                result[i] = a * a / count;
            return result;
        }

        private Arr<int> optimal4Processors(int a)
        {
            Arr<int> result = new Arr<int>(a, a);

            int countPerProcessor = a * a / 4;
            int aDiv2 = a / 2;
            int aMinus1 = a - 1;
            int aSqrt2 = (int)(a / Math.Sqrt(2));

            int toAdd = countPerProcessor - (aSqrt2) * (aSqrt2 - 1) / 2;

            for (int y = 0; y < a; y++)
                for (int x = 0; x < a; x++)
                {
                    int xPlusy = x + y;
                    int xPlusy2 = x + a - y - 1;

                    if (xPlusy <= aSqrt2 - 2 || xPlusy <= aSqrt2 - 1 && y < toAdd)
                    {
                        result[x, y] = 0;
                        continue;
                    }

                    if (2 * a - xPlusy - 1 <= aSqrt2 - 1 || 2 * a - xPlusy - 1 <= aSqrt2 && y >= a - toAdd)
                    {
                        result[x, y] = 1;
                        continue;
                    }

                    if (y < aDiv2)
                        result[x, y] = (xPlusy2 < a) ? 2 : 3;
                    else
                        result[x, y] = (xPlusy2 < a - 1) ? 2 : 3;
                }

            int[] counts = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < result.Length; i++)
                counts[result[i]]++;

            return result;
        }
    }
}
