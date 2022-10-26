using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace OneViewer.Controls
{
    public enum ViewModes
    {
        LargeIcon,
        Details,
        SmallIcon,
        List,
        Tiles
    }

    [TemplatePart(Name = PART_ScrollViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_ItemsPanel, Type = typeof(VirtualizingWrapPanel))]
    [TemplatePart(Name = PART_HeaderRow, Type = typeof(ScrollViewer))]
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListViewItem))]
    public class MultiModeListView : ListBox
    {
        #region Variables
        private const string PART_ScrollViewer = "PART_ScrollViewer";
        private const string PART_ItemsPanel = "PART_ItemsPanel";
        private const string PART_HeaderRow = "PART_HeaderRow";

        private VirtualizingWrapPanel itemPanel = null;
        #endregion

        #region Constructors
        static MultiModeListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiModeListView), new FrameworkPropertyMetadata(typeof(MultiModeListView)));
            SelectionModeProperty.OverrideMetadata(typeof(MultiModeListView), new FrameworkPropertyMetadata(SelectionMode.Extended));
        }

        public MultiModeListView()
            : base()
        {
        }
        #endregion

        #region Properties

        #region EmptyContentProperty
        public static readonly DependencyProperty EmptyContentProperty = DependencyProperty.Register(
            nameof(EmptyContent), typeof(object), typeof(MultiModeListView),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnEmptyContentChanged)));

        [Bindable(true), Category("Content")]
        public object EmptyContent
        {
            get { return GetValue(EmptyContentProperty); }
            set { SetValue(EmptyContentProperty, value); }
        }

        private static void OnEmptyContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiModeListView ctrl = (MultiModeListView)d;
            ctrl.OnEmptyContentChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnEmptyContentChanged(object oldValue, object newValue)
        {
            RemoveLogicalChild(oldValue);
            AddLogicalChild(newValue);
        }

        public static readonly DependencyProperty EmptyTemplateProperty = DependencyProperty.Register(
                "EmptyTemplate", typeof(DataTemplate), typeof(MultiModeListView),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnEmptyTemplateChanged)));

        [Bindable(true), Category("Content")]
        public DataTemplate EmptyTemplate
        {
            get { return (DataTemplate)GetValue(EmptyTemplateProperty); }
            set { SetValue(EmptyTemplateProperty, value); }
        }

        private static void OnEmptyTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiModeListView ctrl = (MultiModeListView)d;
            ctrl.OnEmptyTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnEmptyTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }

        public static readonly DependencyProperty EmptyTemplateSelectorProperty = DependencyProperty.Register(
                nameof(EmptyTemplateSelector), typeof(DataTemplateSelector), typeof(MultiModeListView),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnEmptyTemplateSelectorChanged)));

        [Bindable(true), Category("Content")]
        public DataTemplateSelector EmptyTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(EmptyTemplateSelectorProperty); }
            set { SetValue(EmptyTemplateSelectorProperty, value); }
        }

        private static void OnEmptyTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiModeListView ctrl = (MultiModeListView)d;
            ctrl.OnEmptyTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
        }

        protected virtual void OnEmptyTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
        }

        private static readonly DependencyPropertyKey IsEmptyPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(IsEmpty), typeof(bool), typeof(MultiModeListView),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
        }
        #endregion

        #region ViewModeProperty
        /// <summary>
        ///     ViewMode DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register(
                        nameof(ViewMode), typeof(ViewModes), typeof(MultiModeListView),
                        new FrameworkPropertyMetadata(ViewModes.LargeIcon, new PropertyChangedCallback(OnViewModeChanged)),
                        new ValidateValueCallback(IsValidViewMode));

        /// <summary>
        ///     Indicates the view mode behavior for the MultiModeListView.
        /// </summary>
        public ViewModes ViewMode
        {
            get { return (ViewModes)GetValue(ViewModeProperty); }
            set { SetValue(ViewModeProperty, value); }
        }

        private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiModeListView ctrl = (MultiModeListView)d;

            ctrl.SetValue(HasRowHeaderPropertyKey, (ViewModes)e.NewValue == ViewModes.Details);
            ctrl.SetValue(IsContentModePropertyKey, (ViewModes)e.NewValue != ViewModes.Details);

            ctrl.OnViewModeChanged((ViewModes)e.OldValue, (ViewModes)e.NewValue);
        }

        private static bool IsValidViewMode(object o)
        {
            ViewModes value = (ViewModes)o;
            return value == ViewModes.LargeIcon
                || value == ViewModes.Details
                || value == ViewModes.SmallIcon
                || value == ViewModes.List
                || value == ViewModes.Tiles;
        }

        protected virtual void OnViewModeChanged(ViewModes oldValue, ViewModes newValue)
        {
            ApplyNewView();
        }
        #endregion

        #region HasRowHeaderProperty
        private static readonly DependencyPropertyKey HasRowHeaderPropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(HasRowHeader), typeof(bool), typeof(MultiModeListView),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasRowHeaderProperty = HasRowHeaderPropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool HasRowHeader
        {
            get { return (bool)GetValue(HasRowHeaderProperty); }
        }
        #endregion

        #region IsContentModeProperty
        private static readonly DependencyPropertyKey IsContentModePropertyKey = DependencyProperty.RegisterReadOnly(
                nameof(IsContentMode), typeof(bool), typeof(MultiModeListView),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty IsContentModeProperty = IsContentModePropertyKey.DependencyProperty;

        [Bindable(false), Browsable(false)]
        public bool IsContentMode
        {
            get { return (bool)GetValue(IsContentModeProperty); }
        }
        #endregion

        #region ViewProperty
        /// <summary>
        /// View DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(
                "View", typeof(ViewBase), typeof(MultiModeListView),
                new PropertyMetadata(new PropertyChangedCallback(OnViewChanged)));

        /// <summary>
        /// descriptor of the whole view. Include chrome/layout/item/...
        /// </summary>
        public ViewBase View
        {
            get { return (ViewBase)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }

        private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiModeListView ctrl = (MultiModeListView)d;
            ctrl.ApplyNewView();
        }
        #endregion

        #region LargeIconTemplate
        /// <summary>
        ///     The DependencyProperty for the LargeIconTemplate property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty LargeIconTemplateProperty = DependencyProperty.Register(
                        "LargeIconTemplate", typeof(DataTemplate), typeof(MultiModeListView),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnLargeIconTemplateChanged)));

        /// <summary>
        ///     LargeIconTemplate is the template used to display each item.
        /// </summary>
        [Bindable(true), Category("Content")]
        public DataTemplate LargeIconTemplate
        {
            get { return (DataTemplate)GetValue(LargeIconTemplateProperty); }
            set { SetValue(LargeIconTemplateProperty, value); }
        }

        /// <summary>
        ///     Called when LargeIconTemplateProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnLargeIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnLargeIconTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the LargeIconTemplate property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the LargeIconTemplate property.</param>
        /// <param name="newValue">The new value of the LargeIconTemplate property.</param>
        protected virtual void OnLargeIconTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }


        /// <summary>
        ///     The DependencyProperty for the LargeIconTemplateSelector property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty LargeIconTemplateSelectorProperty = DependencyProperty.Register(
                        "LargeIconTemplateSelector", typeof(DataTemplateSelector), typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnLargeIconTemplateSelectorChanged)));

        /// <summary>
        ///     LargeIconTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display each item.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if <seealso cref="LargeIconTemplate"/> is set.
        /// </remarks>
        [Bindable(true), Category("Content")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTemplateSelector LargeIconTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(LargeIconTemplateSelectorProperty); }
            set { SetValue(LargeIconTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///     Called when LargeIconTemplateSelectorProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnLargeIconTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnLargeIconTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the LargeIconTemplateSelector property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the LargeIconTemplateSelector property.</param>
        /// <param name="newValue">The new value of the LargeIconTemplateSelector property.</param>
        protected virtual void OnLargeIconTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
        }
        #endregion

        #region SmallIconTemplate
        /// <summary>
        ///     The DependencyProperty for the SmallIconTemplate property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty SmallIconTemplateProperty = DependencyProperty.Register(
                        "SmallIconTemplate", typeof(DataTemplate), typeof(MultiModeListView),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSmallIconTemplateChanged)));

        /// <summary>
        ///     SmallIconTemplate is the template used to display each item.
        /// </summary>
        [Bindable(true), Category("Content")]
        public DataTemplate SmallIconTemplate
        {
            get { return (DataTemplate)GetValue(SmallIconTemplateProperty); }
            set { SetValue(SmallIconTemplateProperty, value); }
        }

        /// <summary>
        ///     Called when SmallIconTemplateProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnSmallIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnSmallIconTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the SmallIconTemplate property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the SmallIconTemplate property.</param>
        /// <param name="newValue">The new value of the SmallIconTemplate property.</param>
        protected virtual void OnSmallIconTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }


        /// <summary>
        ///     The DependencyProperty for the SmallIconTemplateSelector property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty SmallIconTemplateSelectorProperty = DependencyProperty.Register(
                        "SmallIconTemplateSelector", typeof(DataTemplateSelector), typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSmallIconTemplateSelectorChanged)));

        /// <summary>
        ///     SmallIconTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display each item.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if <seealso cref="SmallIconTemplate"/> is set.
        /// </remarks>
        [Bindable(true), Category("Content")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTemplateSelector SmallIconTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SmallIconTemplateSelectorProperty); }
            set { SetValue(SmallIconTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///     Called when SmallIconTemplateSelectorProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnSmallIconTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnSmallIconTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the SmallIconTemplateSelector property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the SmallIconTemplateSelector property.</param>
        /// <param name="newValue">The new value of the SmallIconTemplateSelector property.</param>
        protected virtual void OnSmallIconTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
        }
        #endregion

        #region ListItemTemplate
        /// <summary>
        ///     The DependencyProperty for the ListItemTemplate property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty ListItemTemplateProperty = DependencyProperty.Register(
                        "ListItemTemplate", typeof(DataTemplate), typeof(MultiModeListView),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnListItemTemplateChanged)));

        /// <summary>
        ///     ListItemTemplate is the template used to display each item.
        /// </summary>
        [Bindable(true), Category("Content")]
        public DataTemplate ListItemTemplate
        {
            get { return (DataTemplate)GetValue(ListItemTemplateProperty); }
            set { SetValue(ListItemTemplateProperty, value); }
        }

        /// <summary>
        ///     Called when ListItemTemplateProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnListItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnListItemTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the ListItemTemplate property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the ListItemTemplate property.</param>
        /// <param name="newValue">The new value of the ListItemTemplate property.</param>
        protected virtual void OnListItemTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }


        /// <summary>
        ///     The DependencyProperty for the ListItemTemplateSelector property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty ListItemTemplateSelectorProperty = DependencyProperty.Register(
                        "ListItemTemplateSelector", typeof(DataTemplateSelector), typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnListItemTemplateSelectorChanged)));

        /// <summary>
        ///     ListItemTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display each item.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if <seealso cref="ListItemTemplate"/> is set.
        /// </remarks>
        [Bindable(true), Category("Content")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTemplateSelector ListItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ListItemTemplateSelectorProperty); }
            set { SetValue(ListItemTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///     Called when ListItemTemplateSelectorProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnListItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnListItemTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the ListItemTemplateSelector property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the ListItemTemplateSelector property.</param>
        /// <param name="newValue">The new value of the ListItemTemplateSelector property.</param>
        protected virtual void OnListItemTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
        }
        #endregion

        #region TilesTemplate
        /// <summary>
        ///     The DependencyProperty for the TilesTemplate property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty TilesTemplateProperty = DependencyProperty.Register(
                        "TilesTemplate", typeof(DataTemplate), typeof(MultiModeListView),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTilesTemplateChanged)));

        /// <summary>
        ///     TilesTemplate is the template used to display each item.
        /// </summary>
        [Bindable(true), Category("Content")]
        public DataTemplate TilesTemplate
        {
            get { return (DataTemplate)GetValue(TilesTemplateProperty); }
            set { SetValue(TilesTemplateProperty, value); }
        }

        /// <summary>
        ///     Called when TilesTemplateProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnTilesTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnTilesTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the TilesTemplate property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the TilesTemplate property.</param>
        /// <param name="newValue">The new value of the TilesTemplate property.</param>
        protected virtual void OnTilesTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }


        /// <summary>
        ///     The DependencyProperty for the TilesTemplateSelector property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty TilesTemplateSelectorProperty = DependencyProperty.Register(
                        "TilesTemplateSelector", typeof(DataTemplateSelector), typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnTilesTemplateSelectorChanged)));

        /// <summary>
        ///     TilesTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display each item.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if <seealso cref="TilesTemplate"/> is set.
        /// </remarks>
        [Bindable(true), Category("Content")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTemplateSelector TilesTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(TilesTemplateSelectorProperty); }
            set { SetValue(TilesTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///     Called when TilesTemplateSelectorProperty is invalidated on "d."
        /// </summary>
        /// <param name="d">The object on which the property was invalidated.</param>
        /// <param name="e">EventArgs that contains the old and new values for this property</param>
        private static void OnTilesTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiModeListView)d).OnTilesTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the TilesTemplateSelector property changes.
        /// </summary>
        /// <param name="oldValue">The old value of the TilesTemplateSelector property.</param>
        /// <param name="newValue">The new value of the TilesTemplateSelector property.</param>
        protected virtual void OnTilesTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
        }
        #endregion

        #endregion

        #region Methods
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            MultiModeListViewItem lvi = element as MultiModeListViewItem;
            if (lvi != null)
            {
                ViewBase view = View;
                if (view != null)
                {
                    // update default style key
                    lvi.SetDefaultStyleKey(typeof(MultiModeListViewItem));
                }
                else
                {
                    lvi.ClearDefaultStyleKey();
                }
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is MultiModeListViewItem);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiModeListViewItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(PART_ItemsPanel) is VirtualizingWrapPanel grid)
            {
                itemPanel = grid;
            }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            SetValue(IsEmptyPropertyKey, Items.Count == 0);

            if (IsLoaded && ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                // UpdateItemsPerRow(new Size(ActualWidth, ActualHeight));
                UpdateWrapMode();
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            SetValue(IsEmptyPropertyKey, Items.Count == 0);
            ApplyNewView();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            // UpdateItemsPerRow(sizeInfo.NewSize);
            UpdateWrapMode();
        }

        private void UpdateWrapMode()
        {
            itemPanel.SpacingMode = ViewMode == ViewModes.Details ? SpacingMode.None : SpacingMode.Uniform;
            itemPanel.NoWrap = ViewMode == ViewModes.Details;
            itemPanel.UpdateLayout();
        }

        //private void UpdateItemsPerRow(Size availableSize)
        //{
        //    if (itemPanel == null || Items.Count == 0) return;

        //    if (ViewMode == ViewModes.Details)
        //    {
        //        itemPanel.Columns = 1;
        //    }
        //    else
        //    {
        //        var childSize = CalculateChildSize();
        //        if (childSize.IsEmpty)
        //        {
        //            itemPanel.Columns = 1;
        //        }
        //        else
        //        {
        //            int itemsPerRowCount;

        //            if (double.IsInfinity(availableSize.Width))
        //                itemsPerRowCount = Items.Count;
        //            else
        //                itemsPerRowCount = Math.Max(1, (int)Math.Floor(availableSize.Width / childSize.Width));

        //            itemPanel.Columns = itemsPerRowCount;
        //        }
        //    }

        //    itemPanel.UpdateLayout();
        //}

        //private Size CalculateChildSize()
        //{
        //    if (Items.Count == 0)
        //        return new Size(0, 0);

        //    var child = (UIElement)ItemContainerGenerator.ContainerFromIndex(0);
        //    if (child == null)
        //        return Size.Empty;

        //    child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        //    return child.DesiredSize;
        //}

        private void ApplyNewView()
        {
            if (ViewMode == ViewModes.LargeIcon)
            {
                SetValue(ItemTemplateProperty, LargeIconTemplate);
                SetValue(ItemTemplateSelectorProperty, LargeIconTemplateSelector);
            }
            else if (ViewMode == ViewModes.SmallIcon)
            {
                SetValue(ItemTemplateProperty, SmallIconTemplate);
                SetValue(ItemTemplateSelectorProperty, SmallIconTemplateSelector);
            }
            else if (ViewMode == ViewModes.List)
            {
                SetValue(ItemTemplateProperty, ListItemTemplate);
                SetValue(ItemTemplateSelectorProperty, ListItemTemplateSelector);
            }
            else if (ViewMode == ViewModes.Tiles)
            {
                SetValue(ItemTemplateProperty, TilesTemplate);
                SetValue(ItemTemplateSelectorProperty, TilesTemplateSelector);
            }
            else
            {
                SetValue(ItemTemplateProperty, null);
                SetValue(ItemTemplateSelectorProperty, null);
            }

            // Encounter a new view after loaded means user is switching view.
            // Force to regenerate all containers.
            if (IsLoaded)
            {
                // UpdateItemsPerRow(new Size(ActualWidth, ActualHeight));
                UpdateWrapMode();
            }
        }
        #endregion
    }
}
