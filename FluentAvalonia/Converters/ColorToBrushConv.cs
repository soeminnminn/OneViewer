﻿using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using FluentAvalonia.UI.Media;
using System;
using System.Globalization;

namespace FluentAvalonia.Converters
{
    public class ColorToBrushConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color c)
                return new SolidColorBrush(c);

			if (value is Color2 c2)
				return new SolidColorBrush(c2);

            return BindingOperations.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ISolidColorBrush sc)
                return sc.Color;

            return BindingOperations.DoNothing;
        }
    }

	public class ColorShadeBrushConv : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var color = (Color2)value;

			float amount;
			if (!float.TryParse(parameter.ToString(), out amount))
			{
				amount = 0;
			}

			return new SolidColorBrush(color.LightenPercent(amount));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return BindingOperations.DoNothing;
		}
	}
}
