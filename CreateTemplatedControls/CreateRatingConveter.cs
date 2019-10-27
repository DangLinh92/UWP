using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CreateTemplatedControls
{
    class CreateRatingConveter : IValueConverter
    {
        public int CompareValue { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((int)value >= CompareValue) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
