using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OneViewer.ViewModels;
using OneViewer.Common;
using OneViewer.Models;

namespace OneViewer.TemplateSelectors
{
    public class FileListItemTemplateSelector : DataTemplateSelector
    {
        // private static ListView parentListView = null;

        public DataTemplate DirectoryTemplate { get; set; }
        public DataTemplate FileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //if (parentListView == null && container is ContentPresenter presenter)
            //{
            //    parentListView = FindParent(presenter, typeof(ListView)) as ListView;
            //}

            //var viewMode = FileViewModes.Tiles;
            //if (parentListView != null && parentListView.DataContext is MainWindowModel model)
            //{
            //    viewMode = model.ViewMode;
            //}

            //if (viewMode == FileViewModes.Tiles)
            //{
            //    if (item is FileModel)
            //        return TilesFileTemplate;

            //    return TilesDirectoryTemplate;
            //}
            //else if (viewMode == FileViewModes.CompactList)
            //{
            //    if (item is FileModel)
            //        return CompactFileTemplate;

            //    return CompactDirectoryTemplate;
            //}

            // return base.SelectTemplate(item, container);

            if (item is FileModel)
                return FileTemplate;

            return DirectoryTemplate;
        }

        //private static DependencyObject FindParent(DependencyObject reference, Type type)
        //{
        //    var parent = VisualTreeHelper.GetParent(reference);
        //    while (parent != null && parent.GetType() != type)
        //    {
        //        parent = VisualTreeHelper.GetParent(parent);
        //    }

        //    return parent;
        //}
    }
}
