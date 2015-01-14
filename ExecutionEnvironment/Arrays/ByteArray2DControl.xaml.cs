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
    public partial class ByteArray2DControl : UserControl, IDisposable, INotifyPropertyChanged
    {
        Arr<byte> array;
        WriteableBitmap bitmap;

        public Arr<byte> Array
        {
            get { return array; }
            set
            {
                this.array = value;

                int max = array.Max + 1;

                bitmap = BitmapFactory.New(this.array.W * 4, this.array.H * 4);

                Dictionary<int, Color> colorCache = new Dictionary<int, Color>();
                for (int x = 0; x < array.W; x++)
                    for (int y = 0; y < array.H; y++)
                    {
                        int v = array.At(x, y);
                        if (!colorCache.ContainsKey(v))
                            colorCache.Add(v, ColorHelper.GenerateColor(v, max, 1, 1));
                        bitmap.FillRectangle(x * 4, y * 4, x * 4 + 4, y * 4 + 4, colorCache[v]);
                    }

                imageControl.Width = bitmap.Width;
                imageControl.Height = bitmap.Height;
                imageControl.Source = bitmap;
            }
        }

        public ByteArray2DControl()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            imageControl.Source = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
