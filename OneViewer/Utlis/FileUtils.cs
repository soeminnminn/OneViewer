using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace OneViewer.Utils
{
    public static class FileUtils
    {
        /// <summary>
        /// 判断一个文件是否为文本文件
        /// </summary>
        /// <para> From: https://social.msdn.microsoft.com/Forums/vstudio/en-US/c177719a-4671-4435-aa4f-7a92852be6cc/how-can-i-determine-if-a-file-is-binary-or-text-in-c </para>
        /// <param name="encoding"></param>
        /// <param name="filePath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static bool IsTextFile(out Encoding? encoding, string filePath, int windowSize = 10240)
        {
            FileStream fileStream;
            try
            {
                fileStream = File.OpenRead(filePath);
            }
            catch
            {
                encoding = null;
                return false;
            }

            var rawData = new byte[windowSize];
            var text = new char[windowSize];
            var isText = true;

            // Read raw bytes
            var rawLength = fileStream.Read(rawData, 0, rawData.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            // Detect encoding correctly (from Rick Strahl's blog)
            // http://www.west-wind.com/weblog/posts/2007/Nov/28/Detecting-Text-Encoding-for-StreamReader
            encoding = rawData[0] switch
            {
                0x00 when rawData[1] == 0x00 && rawData[2] == 0xfe && rawData[3] == 0xff => Encoding.UTF32,
                // 0x2b when rawData[1] == 0x2f && rawData[2] == 0x76 => Encoding.UTF7,
                0xef when rawData[1] == 0xbb && rawData[2] == 0xbf => Encoding.UTF8,
                0xfe when rawData[1] == 0xff => Encoding.Unicode,
                _ => Encoding.Default
            };

            // Read text and detect the encoding
            using (var streamReader = new StreamReader(fileStream))
            {
                streamReader.Read(text, 0, text.Length);
            }

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, encoding);
            // Write the text to a buffer
            streamWriter.Write(text);
            streamWriter.Flush();

            // Get the buffer from the memory stream for comparision
            var memoryBuffer = memoryStream.GetBuffer();

            // Compare only bytes read
            for (var i = 0; i < rawLength && isText; i++)
            {
                isText = rawData[i] == memoryBuffer[i];
            }

            return isText;
        }

        #region SetAsWallpaper
        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

        private static readonly uint SPI_SETDESKWALLPAPER = 0x14;
        private static readonly uint SPIF_UPDATEINIFILE = 0x01;
        private static readonly uint SPIF_SENDWININICHANGE = 0x02;

        public static void SetWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
        #endregion

        public static ImageSource MediaThumbnail(string mediaFile, int waitTime, int position)
        {
            MediaPlayer player = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };
            player.Open(new Uri(mediaFile));
            player.Pause();
            player.Position = TimeSpan.FromSeconds(position);

            //We need to give MediaPlayer some time to load. 
            //The efficiency of the MediaPlayer depends                 
            //upon the capabilities of the machine it is running on and 
            //would be different from time to time
            System.Threading.Thread.Sleep(waitTime * 1000);

            //120 = thumbnail width, 90 = thumbnail height and 96x96 = horizontal x vertical DPI
            //In an real application, you would not probably use hard coded values!
            RenderTargetBitmap rtb = new RenderTargetBitmap(120, 90, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawVideo(player, new Rect(0, 0, 120, 90));
            }
            rtb.Render(dv);
            player.Close();

            return rtb;

            // Duration duration = player.NaturalDuration;
            //int videoLength = 0;
            //if (duration.HasTimeSpan)
            //{
            //    videoLength = (int)duration.TimeSpan.TotalSeconds;
            //}

            //BitmapFrame frame = BitmapFrame.Create(rtb).GetCurrentValueAsFrozen() as BitmapFrame;
            //BitmapEncoder encoder = new JpegBitmapEncoder();
            //encoder.Frames.Add(frame);

            //MemoryStream memoryStream = new MemoryStream();
            //encoder.Save(memoryStream);
        }

        /*
        private void add_Video_Image(string sFullname_Path_of_Video)
        {
            //----------------< add_Video_Image() >----------------
            //*create mediaplayer in memory and jump to position
            MediaPlayer mediaPlayer = new MediaPlayer();
 
            mediaPlayer.MediaOpened += new EventHandler(mediaplayer_OpenMedia);
            mediaPlayer.ScrubbingEnabled = true;
            mediaPlayer.Open(new Uri(sFullname_Path_of_Video));
            mediaPlayer.Position = TimeSpan.FromSeconds(0);
            //----------------</ add_Video_Image() >----------------
        }
 
        private void mediaplayer_OpenMedia(object sender, EventArgs e)
        {
            //----------------< mediaplayer_OpenMedia() >----------------
            //*create mediaplayer in memory and jump to position
            //< draw video_image >
            MediaPlayer mediaPlayer = sender as MediaPlayer;
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawVideo(mediaPlayer, new Rect(0, 0, 160, 100));
            drawingContext.Close();
 
            double dpiX = 1 / 200;
            double dpiY = 1 / 200;
            RenderTargetBitmap bmp = new RenderTargetBitmap(160, 100, dpiX, dpiY , PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            //</ draw video_image >
 
            //< set Image >
            Image newImage = new Image();
            newImage.Source = bmp;
            newImage.Stretch = Stretch.Uniform;
            newImage.Height = 100;
            //</ set Image >
 
            //< add >
            panel_Images.Children.Add(newImage);
            //</ add >
            //----------------< mediaplayer_OpenMedia() >----------------
        }
        */
    }
}
