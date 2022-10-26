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
    internal class LibraryTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LibraryTypes val)
            {
                switch(val)
                {
                    case LibraryTypes.Desktop:
                        return "PathIconDesktop";
                    case LibraryTypes.Documents:
                        return "PathIconDocument";
                    case LibraryTypes.Downloads:
                        return "PathIconDownload";
                    case LibraryTypes.Music:
                        return "PathIconAudio";
                    case LibraryTypes.Pictures:
                        return "PathIconImage";
                    case LibraryTypes.Videos:
                        return "PathIconMedia";
                    default:
                        break;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
