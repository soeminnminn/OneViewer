using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace OneViewer.Common
{
    public static class RoutedCommands
    {
        public static RoutedUICommand Rename = new RoutedUICommand("Rename", "RenameCommand", typeof(Window));
        public static RoutedUICommand SelectAll = new RoutedUICommand("Select all", "SelectAllCommand", typeof(Window));
        public static RoutedUICommand SortMode = new RoutedUICommand("Sort by", "SortByCommand", typeof(Window));
        public static RoutedUICommand AscDesc = new RoutedUICommand("Asc/Desc", "AscDescCommand", typeof(Window));
        public static RoutedUICommand ViewMode = new RoutedUICommand("View mode", "ViewModeCommand", typeof(Window));
        public static RoutedUICommand Refresh = new RoutedUICommand("Refresh", "RefreshCommand", typeof(Window));
        public static RoutedUICommand Delete = new RoutedUICommand("Delete", "DeleteCommand", typeof(Window));
        public static RoutedUICommand Exit = new RoutedUICommand("Exit", "ExitCommand", typeof(Window));
        public static RoutedUICommand About = new RoutedUICommand("About", "AboutCommand", typeof(Window));

        public static void RaiseClickEvent(this Control sender, ExecutedRoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                (sender as MenuItem)?.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent, e.Source));
            }
            else if (sender is ButtonBase)
            {
                (sender as ButtonBase)?.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, e.Source));
            }
        }
    }
}
