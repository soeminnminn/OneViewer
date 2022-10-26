using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using OneViewer.Common;

namespace OneViewer.Converters
{
    public class ViewModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FileViewModes viewMode = FileViewModes.Tiles;

            if (value is FileViewModes mode)
                viewMode = mode;
            else if (value is string str && Enum.TryParse(typeof(FileViewModes), str, out object val)
                    && val is FileViewModes vm)
                viewMode = vm;
            else if (value is bool isList)
                viewMode = isList ? FileViewModes.List : FileViewModes.CompactList;

            if (viewMode == FileViewModes.List)
                return Controls.ViewModes.Details;
            else if (viewMode == FileViewModes.CompactList)
                return Controls.ViewModes.Tiles;

            return Controls.ViewModes.LargeIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
