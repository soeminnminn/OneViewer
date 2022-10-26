using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace OneViewer.Controls
{
    public class DropdownButton : ComboBox
    {
        #region Variables
        #endregion

        #region Constructors
        static DropdownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropdownButton), new FrameworkPropertyMetadata(typeof(DropdownButton)));
        }

        public DropdownButton()
            : base()
        {
            DefaultStyleKey = typeof(DropdownButton);
        }
        #endregion

        #region Properties
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
                nameof(Header), typeof(object), typeof(DropdownButton),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnHeaderChanged)));

        [Bindable(true), Category("Content")]
        [Localizability(LocalizationCategory.Label)]
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownButton ctrl = (DropdownButton)d;

            ctrl.SetValue(HasHeaderPropertyKey, (e.NewValue != null) ? true : false);
            ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnHeaderChanged(object oldValue, object newValue)
        {
            RemoveLogicalChild(oldValue);
            AddLogicalChild(newValue);
        }

        private static readonly DependencyPropertyKey HasHeaderPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasHeader), typeof(bool), typeof(DropdownButton),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasHeaderProperty = HasHeaderPropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool HasHeader
        {
            get { return (bool)GetValue(HasHeaderProperty); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
                "HeaderTemplate", typeof(DataTemplate), typeof(DropdownButton),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnHeaderTemplateChanged)));

        [Bindable(true), Category("Content")]
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownButton ctrl = (DropdownButton)d;
            ctrl.OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnHeaderTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
                nameof(HeaderTemplateSelector), typeof(DataTemplateSelector), typeof(DropdownButton),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnHeaderTemplateSelectorChanged)));

        [Bindable(true), Category("Content")]
        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }

        private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownButton ctrl = (DropdownButton)d;
            ctrl.OnHeaderTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
        }

        protected virtual void OnHeaderTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
        }
        #endregion

        #region Methods
        internal void NotifyDropdownItemMouseDown(DropdownButtonItem item)
        {
        }

        internal void NotifyDropdownItemMouseUp(DropdownButtonItem item)
        {
            Close();
        }

        internal void NotifyDropdownItemEnter(DropdownButtonItem item)
        {
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DropdownButtonItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DropdownButtonItem;
        }

        private void Close()
        {
            if (IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, false);
            }
        }
        #endregion
    }
}
