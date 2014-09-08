using DrawingSupport;
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

namespace DataFieldLayoutSimulation
{
    /// <summary>
    /// Interaction logic for TwoDArrayControl.xaml
    /// </summary>
    public partial class StencilSpeciesControl : UserControl
    {
        StencilSpecies specimen;

        public StencilSpecies Item
        {
            get { return specimen; }
            set
            {
                this.specimen = value;

                Draw.Clear(surface);
                for (int x = 0; x < specimen.Creator.FieldW; x++)
                    for (int y = 0; y < specimen.Creator.FieldH; y++)
                    {
                        var brush = ColorHelper.GenerateBrush(specimen.At(x, y), specimen.Creator.CellsPerProcessor.Length, 1, 1);
                        Draw.DrawRect(surface, x * 4, y * 4, 4, 4, brush, brush);
                    }
            }
        }

        public StencilSpeciesControl()
        {
            InitializeComponent();
        }
    }
}
