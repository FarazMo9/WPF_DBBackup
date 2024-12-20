﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Presentation.Converter
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            if (value is bool)
            {
                var attended = System.Convert.ToBoolean(value);
                if (attended)
                    return Visibility.Visible;
                return Visibility.Hidden;

            }
            else
                return Visibility.Visible;


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value;
            }
            return Visibility.Collapsed;
        }
    }
}
