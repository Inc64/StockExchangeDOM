using StockExchangeDOM.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace StockExchangeDOM.ViewModel.Converters
{
    public class OfferToColorConverter : IValueConverter
    {
        private static Brush bidBrush = Brushes.LightGreen;
        private static Brush askBrush = Brushes.LightSalmon;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((eTickerOffer)value) == eTickerOffer.bid ? bidBrush : askBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
