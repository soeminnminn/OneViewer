using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OneViewer.Converters
{
    public class BooleanObjectConverter : IValueConverter
    {
        public object Value { get; set; }

        public bool IsInvert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
                return ConvertVisibility(Value, parameter, value);

            var result = IsEqual(Value, parameter, value);
            return IsInvert ? !result : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
                return ConvertVisibility(Value, parameter, value);
            else if (BooleanOf(value))
            {
                if (Value != null)
                    return Value;

                if (parameter != null)
                    return parameter;
            }
            return null;
        }

        private bool IsEqual(object given, object parameter, object value)
        {
            if (value == null) return false;
            if (given == null)
            {
                if (parameter != null)
                {
                    if (parameter.GetType() == value.GetType())
                        return parameter == value;
                    else
                        return $"{parameter}" == $"{value}";
                }
                return true;
            }

            if (given.GetType() == value.GetType())
                return given == value;
            else
                return $"{given}" == $"{value}";
        }

        private static bool BooleanOf(object value)
        {
            if (value is bool b) 
                return b;
            
            if (value is string str) 
                return string.Equals(str, "true", StringComparison.InvariantCultureIgnoreCase);

            return value == null;
        }

        private static Visibility VisibilityOf(object value, Visibility fallback = Visibility.Collapsed)
        {
            if (value is Visibility visibility) return visibility;
            if (value is string str && Enum.TryParse(str, out Visibility o)) return o;
            return fallback;
        }

        private Visibility ConvertVisibility(object given, object parameter, object value)
        {
            var notVisibleValue = VisibilityOf(given);

            if (parameter != null)
                notVisibleValue = VisibilityOf(parameter);

            if (value == null)
                return IsInvert ? notVisibleValue : Visibility.Visible;

            bool visible;
            if (value != null && bool.TryParse(value.ToString(), out bool b))
                visible = b;
            else
                visible = IsEqual(given, parameter, value);

            visible = IsInvert ? !visible : visible;
            return visible ? Visibility.Visible : notVisibleValue;
        }
    }
}
