using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OneViewer.Factories;

namespace OneViewer.Controls
{
    public class PathIcon : Control
    {
        #region Variables
        private static readonly Lazy<IDictionary<string, Geometry>> _dataIndex
            = new Lazy<IDictionary<string, Geometry>>(IconsDataFactory.Create);
        #endregion

        #region Constructors
        static PathIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathIcon), new FrameworkPropertyMetadata(typeof(PathIcon)));
        }

        public PathIcon()
        {
            DefaultStyleKey = typeof(PathIcon);
        }
        #endregion

        #region Properties
        public static readonly DependencyProperty KindProperty
           = DependencyProperty.Register(nameof(Kind), typeof(string), typeof(PathIcon), 
               new PropertyMetadata(string.Empty, OnKindPropertyChanged));

        private static void OnKindPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PathIcon)d).UpdateData();
        }

        /// <summary>
        /// Gets or sets the icon to display.
        /// </summary>
        public string Kind
        {
            get => (string)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        private static readonly DependencyPropertyKey DataPropertyKey
            = DependencyProperty.RegisterReadOnly(nameof(Data), typeof(Geometry), typeof(PathIcon), 
                new PropertyMetadata(null, OnDataPropertyChanged));

        // ReSharper disable once StaticMemberInGenericType
        public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;

        private static void OnDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (PathIcon)d;
            ctrl.SetValue(HasDataPropertyKey, (e.NewValue != null) ? true : false);
            ctrl.OnDataPropertyChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Gets the icon path data for the current <see cref="Kind"/>.
        /// </summary>
        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            private set => SetValue(DataPropertyKey, value);
        }

        protected virtual void OnDataPropertyChanged(object oldValue, object newValue)
        {

        }

        private static readonly DependencyPropertyKey HasDataPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasData), typeof(bool), typeof(PathIcon),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasDataProperty = HasDataPropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool HasData
        {
            get { return (bool)GetValue(HasDataProperty); }
        }


        public static readonly DependencyProperty SourceProperty
           = DependencyProperty.Register(nameof(Source), typeof(string), typeof(PathIcon),
               new PropertyMetadata(string.Empty, OnSourcePropertyChanged));

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (PathIcon)d;
            ctrl.SetValue(HasSourcePropertyKey, (e.NewValue != null || e.NewValue == (object)string.Empty) ? true : false);
            ctrl.OnSourcePropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

        [TypeConverter(typeof(GeometryConverter))]
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            private set => SetValue(SourceProperty, value);
        }

        protected virtual void OnSourcePropertyChanged(string oldValue, string newValue)
        {

        }

        private static readonly DependencyPropertyKey HasSourcePropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasSource), typeof(bool), typeof(PathIcon),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasSourceProperty = HasSourcePropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool HasSource
        {
            get { return (bool)GetValue(HasSourceProperty); }
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateData();
        }

        private void UpdateData()
        {
            Geometry data = null;
            _dataIndex.Value?.TryGetValue(Kind, out data);
            Data = data;
        }
        #endregion
    }
}
