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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EvolutionControl : UserControl, INotifyPropertyChanged
    {
        public Evolution Evolution { get { return (Evolution)this.DataContext; } set { this.DataContext = value; breedingPoolControl.BreedingPool = value; } }

        public EvolutionControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Evolution.Pause = !Evolution.Pause;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
