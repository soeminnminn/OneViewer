using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OneViewer.Controls
{
    public class BreadcrumbItem : ButtonBase
    {
        #region Constructors
        static BreadcrumbItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BreadcrumbItem), new FrameworkPropertyMetadata(typeof(BreadcrumbItem)));
        }

        public BreadcrumbItem()
            : base()
        {
            
        }
        #endregion

        #region Properties
        public bool IsSelected
        {
            get => (bool)GetValue(Selector.IsSelectedProperty);
        }

        internal Breadcrumb ParentBreadcrumb
        {
            get => (Breadcrumb)ItemsControl.ItemsControlFromItemContainer(this);
        }
        #endregion

        #region Methods
        protected override void OnClick()
        {
            base.OnClick();
            if (Breadcrumb.ItemSelectedCommand != null)
            {
                Breadcrumb.ItemSelectedCommand.Execute(Content, this);
            }
        }
        #endregion
    }
}
