using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace OneViewer.Controls.Preview
{
    /// <summary>
    /// Interaction logic for TextPreviewPopup.xaml
    /// </summary>
    public partial class TextPreviewPopup : UserControl
    {
        private readonly Dictionary<string, double> viewHistory = new();

        private readonly byte[] buffer = new byte[10240];

        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
        nameof(FilePath), typeof(string), typeof(TextPreviewPopup), new PropertyMetadata(default(string)));

        public string FilePath
        {
            get => (string)GetValue(FilePathProperty);
            set => SetValue(FilePathProperty, value);
        }

        public TextPreviewPopup()
        {
            InitializeComponent();
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            if (FilePath != null)
            {
                viewHistory[FilePath] = ScrollViewer.VerticalOffset;
            }
            FilePath = filePath;
            StatusTextBlock.Text = Path.GetFileName(filePath);
            if (Utils.FileUtils.IsTextFile(out var encoding, filePath))
            {
                TextBlock.Foreground = new SolidColorBrush(Color.FromRgb(0xe6, 0xe6, 0xe6));
                using var fs = File.OpenRead(filePath);
                var length = fs.Read(buffer);
                TextBlock.Text = encoding.GetString(buffer, 0, length);
                if (viewHistory.TryGetValue(filePath, out var offsetY))
                {
                    ScrollViewer.ScrollToVerticalOffset(offsetY);
                }
                else
                {
                    ScrollViewer.ScrollToHome();
                }
            }
            else
            {
                TextBlock.Foreground = Brushes.Yellow;
                TextBlock.Text = "Unsupported encoding";
            }
        }

        public void Close()
        {
            TextBlock.Text = null;
            if (FilePath != null)
            {
                viewHistory[FilePath] = ScrollViewer.VerticalOffset;
                FilePath = null;
            }
        }

        public void HandleMouseScroll(MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
        }
    }
}
