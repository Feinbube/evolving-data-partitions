using DrawingSupport;
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

namespace ExecutionEnvironment
{
    /// <summary>
    /// Interaction logic for IntArray2DControl.xaml
    /// </summary>
    public partial class DoubleArray1DControl : UserControl, IDisposable, INotifyPropertyChanged
    {
        Arr<double> array;
        WriteableBitmap bitmap;

        public int DisplayHeight { get; set; }

        public Arr<double> Array
        {
            get { return array; }
            set
            {
                this.array = value;

                bitmap = BitmapFactory.New(array.Length + 20, DisplayHeight);

                if (array.Length > 0)
                {
                    double min = array.Min;
                    double max = array.Max;
                    
                    for (int i = 0; i < array.Length; i++)
                        bitmap.DrawLine(10 + 2* i, DisplayHeight - 10, 10 + 2* i, (int)(DisplayHeight - 10 - (DisplayHeight - 20) * scale(min, max, array[i])), Colors.Black);
                }

                imageControl.Width = bitmap.Width;
                imageControl.Height = bitmap.Height;
                imageControl.Source = bitmap;
            }
        }

        private double scale(double min, double max, double value)
        {
            return min == max ? 1.0 : Math.Abs((value - min) / (min - max));
        }

        public DoubleArray1DControl()
        {
            DisplayHeight = 90;

            InitializeComponent();
        }

        public void Dispose()
        {
            imageControl.Source = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
