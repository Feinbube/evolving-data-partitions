using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EvolutionWpfControls.ValueConverters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class DoubleToPercentageConverter : BaseConverter, IValueConverter
    {
        public DoubleToPercentageConverter() { }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((int)(((double)value) * 1000)) / 10.0).ToString() + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return double.Parse(((string)value).TrimEnd('%')) / 100.0;
        }
    }
}
