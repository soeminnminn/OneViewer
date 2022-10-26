using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OneViewer.Controls
{
    [TemplatePart(Name = PART_MoreMenuContainer, Type = typeof(StackPanel))]
    [TemplatePart(Name = PART_MoreMenu, Type = typeof(Menu))]
    public class Breadcrumb : Selector
    {
        #region Variables
        private const string PART_MoreMenu = "PART_MoreMenu";
        private const string PART_MoreMenuContainer = "PART_MoreMenuContainer";

        private static RoutedUICommand itemSelectedCommand = new RoutedUICommand("Select item", "SelectItemCommand", typeof(Breadcrumb));
        private static RoutedUICommand invokeMenuCommand = new RoutedUICommand("Open menu", "InvokeMenuCommand", typeof(Breadcrumb));

        private StackPanel moreMenuContainer = null;
        private Menu moreMenu = null;
        #endregion

        #region Constructors
        static Breadcrumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Breadcrumb), new FrameworkPropertyMetadata(typeof(Breadcrumb)));
        }

        public Breadcrumb()
            : base()
        {
            DefaultStyleKey = typeof(Breadcrumb);

            CommandBindings.Add(new CommandBinding(ItemSelectedCommand, SelectItem_Executed));
        }
        #endregion

        #region Properties
        private static readonly DependencyPropertyKey HasMoreItemPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasMoreItem), typeof(bool), typeof(Breadcrumb),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasMoreItemProperty = HasMoreItemPropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool HasMoreItem
        {
            get { return (bool)GetValue(HasMoreItemProperty); }
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            nameof(IsExpanded), typeof(bool), typeof(Breadcrumb), 
            new UIPropertyMetadata(false, (s, e) => ((Breadcrumb)s).OnIsExpandedChanged((bool)e.OldValue, (bool)e.NewValue)));

        [Bindable(false), Browsable(false)]
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        private void OnIsExpandedChanged(bool oldValue, bool newValue)
        {

        }

        public static RoutedUICommand ItemSelectedCommand
        {
            get { return itemSelectedCommand; }
        }

        public static RoutedUICommand InvokeMenuCommand
        {
            get { return invokeMenuCommand; }
        }
        #endregion

        #region Methods
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BreadcrumbItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is BreadcrumbItem;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(PART_MoreMenuContainer) is StackPanel stackPanel)
            {
                moreMenuContainer = stackPanel;
            }

            if (GetTemplateChild(PART_MoreMenu) is Menu menu)
            {
                moreMenu = menu;
                menu.CommandBindings.Add(new CommandBinding(InvokeMenuCommand, SelectItem_Executed));
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (IsInitialized && ActualWidth > 0)
            {
                UpdateOverflowItems(ActualWidth);
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateOverflowItems(sizeInfo.NewSize.Width);
        }

        private void UpdateOverflowItems(double width)
        {
            if (!IsInitialized)
                return;

            if (moreMenu != null && Items.Count > 0)
            {
                double childWidth = moreMenuContainer != null ? moreMenuContainer.ActualWidth : 0;
                int count = Items.Count;

                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (ItemContainerGenerator.ContainerFromIndex(i) is BreadcrumbItem container)
                    {
                        container.Visibility = Visibility.Visible;
                        childWidth += container.ActualWidth;

                        if (childWidth > width)
                        {
                            container.Visibility = Visibility.Collapsed;
                        }
                        else
                            count--;
                    }
                }

                SetValue(HasMoreItemPropertyKey, childWidth > width);

                if (childWidth > width)
                {
                    var mmItem = moreMenu.Items[0] as MenuItem;
                    mmItem.ItemsSource = Items.OfType<object>().Take(count).Reverse();
                }
            }
        }

        private void SelectItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter != null)
            {
                var item = ItemContainerGenerator.ContainerFromItem(e.Parameter);
                SetIsSelected(item, true);

                SelectedItem = e.Parameter;
            }
        }
        #endregion
    }
}
