using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PaintToolCs
{
    /// <summary>
    /// value converter to convert with a units label
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class UnitsValueConverter : IValueConverter
    {
        /// <summary>
        /// sets the units label to be used
        /// </summary>
        public string Units
        {
            get;
            set;
        }

        /// <summary>
        /// conversion forward (double > text) is supported
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double doubleValue = System.Convert.ToDouble(value);
            string result = string.Format("{0,6:F2} {1}", doubleValue, Units);
            return result;
        }

        /// <summary>
        /// conversion backward (text > double) is not supported
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
