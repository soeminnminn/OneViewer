﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Input;

namespace OneViewer.Controls.Preview
{
    /// <summary>
    /// Interaction logic for VideoPreviewPopup.xaml
    /// </summary>
    public partial class VideoPreviewPopup : UserControl
    {
        private readonly Dictionary<Uri, TimeSpan> playHistory = new();
        private MediaTimeline timeline;
        private ClockController controller;

        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
        nameof(FilePath), typeof(string), typeof(VideoPreviewPopup), new PropertyMetadata(default(string)));

        public string FilePath
        {
            get => (string)GetValue(FilePathProperty);
            set => SetValue(FilePathProperty, value);
        }

        public VideoPreviewPopup()
        {
            InitializeComponent();
        }

        private void Timeline_OnCurrentTimeInvalidated(object sender, EventArgs e)
        {
            if (controller == null)
            {
                return;
            }
            if (controller.Clock.NaturalDuration.HasTimeSpan && controller.Clock.CurrentTime.HasValue)
            {
                var currentTime = controller.Clock.CurrentTime.Value;
                var naturalDuration = controller.Clock.NaturalDuration.TimeSpan;
                CurrentTimeTextBlock.Text = currentTime.ToString(@"hh\:mm\:ss");
                TotalTimeTextBlock.Text = naturalDuration.ToString(@"hh\:mm\:ss");
                var ratio = currentTime / naturalDuration;
                TimeSlider.Value = ratio;
            }
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            FilePath = filePath;
            StatusTextBlock.Text = "Loading";
            if (timeline != null && timeline.Source != null && controller != null)
            {
                playHistory[timeline.Source] = controller.Clock.CurrentTime.GetValueOrDefault();
                controller.Stop();
            }
            var newSource = new Uri(filePath, UriKind.Absolute);
            timeline = new MediaTimeline(newSource)
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            timeline.CurrentTimeInvalidated += Timeline_OnCurrentTimeInvalidated;
            var clock = VideoPlayer.Clock = timeline.CreateClock();
            controller = clock.Controller;
        }

        public void Close()
        {
            var source = VideoPlayer.Source;
            if (source != null && controller != null)
            {
                playHistory[source] = controller.Clock.CurrentTime.GetValueOrDefault();
                controller.Stop();
            }
            FilePath = null;
            timeline = null;
            controller = null;
        }

        public void HandleMouseScroll(MouseWheelEventArgs e)
        {
            if (controller != null && controller.Clock.NaturalDuration.HasTimeSpan)
            {
                var offset = controller.Clock.CurrentTime.GetValueOrDefault().Add(TimeSpan.FromMilliseconds(-e.Delta * 10));
                if (offset < TimeSpan.Zero)
                {
                    offset = TimeSpan.Zero;
                }
                else if (offset > controller.Clock.NaturalDuration.TimeSpan)
                {
                    offset = controller.Clock.NaturalDuration.TimeSpan;
                }
                controller?.Seek(offset, TimeSeekOrigin.BeginTime);
            }
        }
        private void VideoPlayer_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            if (timeline == null || timeline.Source == null)
            {
                return;
            }
            if (playHistory.TryGetValue(timeline.Source, out var ts))
            {
                controller.Seek(ts, TimeSeekOrigin.BeginTime);
            }
            StatusTextBlock.Text = Path.GetFileName(FilePath);
        }

    }
}
