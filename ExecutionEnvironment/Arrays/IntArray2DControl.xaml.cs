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

namespace ExecutionEnvironment
{
    /// <summary>
    /// Interaction logic for IntArray2DControl.xaml
    /// </summary>
    public partial class IntArray2DControl : UserControl
    {
        Arr<int> array;

        public Arr<int> Array
        {
            get { return array; }
            set
            {
                this.array = value;

                int max = array.Max + 1;

                Draw.Clear(surface);
                for (int x = 0; x < array.W; x++)
                    for (int y = 0; y < array.H; y++)
                    {
                        var brush = ColorHelper.GenerateBrush(array.At(x, y), max, 1, 1);
                        Draw.DrawRect(surface, x * 4, y * 4, 4, 4, brush, brush);
                    }
            }
        }

        public IntArray2DControl()
        {
            InitializeComponent();
        }
    }
}
