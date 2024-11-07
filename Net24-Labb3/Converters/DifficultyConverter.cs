using Net24_Labb3.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Net24_Labb3.Converters
{
    class DifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Difficulty difficulty)
        {
                return difficulty.ToString();
            }
            return Difficulty.Medium.ToString();
        }
    }
}
