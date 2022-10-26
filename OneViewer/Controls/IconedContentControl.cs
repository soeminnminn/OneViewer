using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OneViewer.Controls
{
    public class IconedContentControl : ContentControl
    {
        #region Constructors
        static IconedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconedContentControl), new FrameworkPropertyMetadata(typeof(IconedContentControl)));
        }

        public IconedContentControl()
            : base()
        {
            DefaultStyleKey = typeof(IconedContentControl);
        }
        #endregion

        #region Properties
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
                nameof(Icon), typeof(object), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnIconChanged)));

        [Bindable(true), Category("Content")]
        [Localizability(LocalizationCategory.Label)]
        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconedContentControl ctrl = (IconedContentControl)d;

            ctrl.SetValue(HasIconPropertyKey, (e.NewValue != null) ? true : false);
            ctrl.OnIconChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnIconChanged(object oldValue, object newValue)
        {
            RemoveLogicalChild(oldValue);
            AddLogicalChild(newValue);
        }

        private static readonly DependencyPropertyKey HasIconPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasIcon), typeof(bool), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasIconProperty = HasIconPropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool HasIcon
        {
            get { return (bool)GetValue(HasIconProperty); }
        }

        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
                "IconTemplate", typeof(DataTemplate), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnIconTemplateChanged)));

        [Bindable(true), Category("Content")]
        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }

        private static void OnIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconedContentControl ctrl = (IconedContentControl)d;
            ctrl.OnIconTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnIconTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }

        public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register(
                nameof(IconTemplateSelector), typeof(DataTemplateSelector), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnIconTemplateSelectorChanged)));

        [Bindable(true), Category("Content")]
        public DataTemplateSelector IconTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(IconTemplateSelectorProperty); }
            set { SetValue(IconTemplateSelectorProperty, value); }
        }

        private static void OnIconTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconedContentControl ctrl = (IconedContentControl)d;
            ctrl.OnIconTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
        }

        protected virtual void OnIconTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
        }
        
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
                nameof(Orientation), typeof(Orientation), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(Orientation.Horizontal, OnOrientationChanged));

        [Bindable(true), Category("Content")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconedContentControl ctrl = (IconedContentControl)d;
            ctrl.OnOrientationChanged((Orientation)e.OldValue, (Orientation)e.NewValue);
        }

        protected virtual void OnOrientationChanged(Orientation oldValue, Orientation newValue)
        {
            UpdateSpacingThickness(newValue, Spacing);
        }

        public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
                nameof(Spacing), typeof(double), typeof(IconedContentControl),
                new FrameworkPropertyMetadata((double)4, OnSpacingChanged));

        [Bindable(true), Category("Content")]
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconedContentControl ctrl = (IconedContentControl)d;
            ctrl.OnSpacingChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual void OnSpacingChanged(double oldValue, double newValue)
        {
            UpdateSpacingThickness(Orientation, newValue);
        }

        private static readonly DependencyPropertyKey SpacingThicknessPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(SpacingThickness), typeof(Thickness), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(new Thickness()));

        public static readonly DependencyProperty SpacingThicknessProperty = SpacingThicknessPropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public Thickness SpacingThickness
        {
            get { return (Thickness)GetValue(SpacingThicknessProperty); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
                nameof(CornerRadius), typeof(CornerRadius), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(new CornerRadius()));

        [Bindable(true), Category("Content")]
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
                nameof(IsSelected), typeof(bool), typeof(IconedContentControl),
                new FrameworkPropertyMetadata(false, OnIsSelectedChanged));

        [Bindable(true), Category("Content")]
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IconedContentControl ctrl = (IconedContentControl)d;
            ctrl.OnIsSelectedChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnIsSelectedChanged(bool oldValue, bool newValue)
        {

        }
        #endregion

        #region Methods
        private void UpdateSpacingThickness(Orientation orientation, double spacing)
        {
            var padding = Padding;
            Thickness thickness;
            if (orientation == Orientation.Horizontal)
                thickness = new Thickness(spacing, padding.Top, padding.Right, padding.Bottom);
            else
                thickness = new Thickness(padding.Left, spacing, padding.Right, padding.Bottom);

            SetValue(SpacingThicknessPropertyKey, thickness);
        }
        #endregion
    }
}
