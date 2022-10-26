using System;
using System.Windows;
using System.Windows.Controls;

namespace OneViewer.Controls
{
    public enum AuraIconButtonType
    {
        NavigateBefore,
        NavigateNext,
        ClockwiseRotate,
        CounterClockwiseRotate
    }

    public class AuraCircleIconButton : Button
    {
        static AuraCircleIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AuraCircleIconButton), new FrameworkPropertyMetadata(typeof(AuraCircleIconButton)));
        }

        public static readonly DependencyProperty AuraIconTypeProperty = DependencyProperty.Register(
            nameof(AuraIconType), typeof(AuraIconButtonType), typeof(AuraCircleIconButton), 
            new FrameworkPropertyMetadata(AuraIconButtonType.NavigateBefore));

        public AuraIconButtonType AuraIconType
        {
            get => (AuraIconButtonType)GetValue(AuraIconTypeProperty);
            set { SetValue(AuraIconTypeProperty, value); }
        }
    }
}
