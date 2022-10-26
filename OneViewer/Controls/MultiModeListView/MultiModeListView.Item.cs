using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OneViewer.Controls
{
    public class MultiModeListViewItem : ListViewItem
    {
        internal void SetDefaultStyleKey(object key)
        {
            DefaultStyleKey = key;
        }

        internal void ClearDefaultStyleKey()
        {
            ClearValue(DefaultStyleKeyProperty);
        }

        internal MultiModeListView ParentMultiModeListView
        {
            get => (MultiModeListView)ItemsControl.ItemsControlFromItemContainer(this);
        }
    }
}
