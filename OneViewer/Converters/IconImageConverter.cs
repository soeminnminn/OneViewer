using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace OneViewer.Converters
{
    public class IconImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (!(value is WindowIcon icon))
            //    return null;

            //using (var stream = new MemoryStream())
            //{
            //    icon.Save(stream);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    return new Bitmap(stream);
            //}
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
