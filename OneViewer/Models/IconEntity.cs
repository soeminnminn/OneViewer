using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using OneViewer.Common;
using Svg;
using System.Drawing.Imaging;

namespace OneViewer.Models
{
    internal class IconEntity : IDisposable
    {
        #region Properties
        #endregion

        #region Constructor
        public IconEntity(FileSystemTypes systemType, string extension = "")
        {
            
        }
        #endregion

        #region Methods
        private static Bitmap SvgToBitmap(Stream fileStream)
        {
            var svgDocument = SvgDocument.Open<SvgDocument>(fileStream);
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
            return null;
        }

        public Bitmap ToBitmap()
        {
            return null;
        }

        public void Dispose()
        {
            //
        }
        #endregion
    }
}
