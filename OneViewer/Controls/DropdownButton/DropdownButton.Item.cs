using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneViewer.Controls
{
    public class DropdownButtonItem : MenuItem
    {
        #region Constructors
        static DropdownButtonItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropdownButtonItem), new FrameworkPropertyMetadata(typeof(DropdownButtonItem)));
        }

        public DropdownButtonItem()
            : base()
        {
            DefaultStyleKey = typeof(DropdownButtonItem);
        }
        #endregion

        #region Properties

        internal DropdownButton ParentDropdownButton
        {
            get => (DropdownButton)ItemsControlFromItemContainer(this);
        }
        #endregion

        #region Methods
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;

            var parent = ParentDropdownButton;

            if (parent != null)
            {
                parent.NotifyDropdownItemMouseDown(this);
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;

            var parent = ParentDropdownButton;
            if (parent != null)
            {
                parent.NotifyDropdownItemMouseUp(this);
            }

            base.OnMouseLeftButtonUp(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            e.Handled = true;

            var parent = ParentDropdownButton;
            if (parent != null)
            {
                parent.NotifyDropdownItemEnter(this);
            }

            base.OnMouseEnter(e);

        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;

            var parent = ParentDropdownButton;
            if (parent != null)
            {
                parent.NotifyDropdownItemEnter(this);
            }

            base.OnGotKeyboardFocus(e);
        }
        #endregion
    }
}
