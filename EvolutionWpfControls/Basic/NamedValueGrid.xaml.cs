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
    /// Interaction logic for NameValueGrid.xaml
    /// </summary>
    public partial class NamedValueGrid : UserControl, IDisposable, INotifyPropertyChanged
    {
        public List<NamedValue> NamedValues { get; set; }

        public int Rows { get; set; }

        public int Columns { get { return NamedValues.Count % Rows; } }

        public NamedValueGrid()
        {
            Rows = 3;
            NamedValues = new List<NamedValue>();

            InitializeComponent();
        }

        public NamedValueGrid(IEnumerable<NamedValue> namedValues) : this() { AddRange(namedValues); }

        public void Add(string name, object value)
        {
            if (NamedValues.Count % Rows == 0)
            {
                stack.Children.Add(new StackPanel() { Margin = new Thickness(0, 0, 20, 0), Orientation = Orientation.Horizontal });
                (stack.Children[stack.Children.Count - 1] as StackPanel).Children.Add(new StackPanel());
                (stack.Children[stack.Children.Count - 1] as StackPanel).Children.Add(new StackPanel());
            }

            ((stack.Children[stack.Children.Count - 1] as StackPanel).Children[0] as StackPanel).Children.Add(new Label() { Content = name, Margin = new Thickness(0, -5, 0, 0) });
            ((stack.Children[stack.Children.Count - 1] as StackPanel).Children[1] as StackPanel).Children.Add(new Label() { Content = value, Margin = new Thickness(0, -5, 0, 0) });

            NamedValues.Add(new NamedValue() { Name = name, Value = value });
        }

        public void AddRange(IEnumerable<NamedValue> namedValues)
        {
            foreach (var namedValue in namedValues)
                Add(namedValue.Name, namedValue.Value);
        }

        public void Dispose()
        {
            NamedValues.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
