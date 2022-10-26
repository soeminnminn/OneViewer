using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OneViewer.Controls.Preview
{
    /// <summary>
    /// Interaction logic for ImagePreviewPopup.xaml
    /// </summary>
    public partial class ImagePreviewPopup : UserControl
    {
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
        nameof(FilePath), typeof(string), typeof(ImagePreviewPopup), new PropertyMetadata(default(string)));

        public string FilePath
        {
            get => (string)GetValue(FilePathProperty);
            set => SetValue(FilePathProperty, value);
        }

        public ImagePreviewPopup()
        {
            InitializeComponent();
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            FilePath = filePath;
            Image.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
        }

        public void Close()
        {
            Image.Source = null;
            FilePath = null;
        }
    }
}
