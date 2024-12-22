using QuanLiCoffeeShop.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLiCoffeeShop.Converters
{
    public class CusNameConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            if (value == null || ((CUSTOMER)value).CUS_NAME == null
                )
            {
                string st = "Khách vãng lai";
                return st;
            }
            else
            {

                return ((CUSTOMER)value).CUS_NAME;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
