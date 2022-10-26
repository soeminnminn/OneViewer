using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Svg;

namespace OneViewer.Converters
{
    internal class FileEntityToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Bitmap dravingImage = null;

            if (!(value is Models.FileSystemObjectModel model))
                return dravingImage;

            var iconPath = model.GetIconPath();
            if (Path.GetExtension(iconPath).ToLower() == ".svg")
            {
                var svgDocument = SvgDocument.Open(iconPath);
                if (svgDocument != null)
                {
                    var bitmap = svgDocument.Draw(256, 256);
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        stream.Seek(0, SeekOrigin.Begin);
                        return new Bitmap(stream);
                    }   
                }
            }
            else
            {
                return new Bitmap(iconPath);
            }

            return dravingImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
