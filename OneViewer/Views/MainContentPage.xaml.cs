using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneViewer.Views
{
    /// <summary>
    /// Interaction logic for MainContentPage.xaml
    /// </summary>
    public partial class MainContentPage : Page
    {
        public MainContentPage()
        {
            InitializeComponent();
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainWindowModel model)
            {
                e.CanExecute = model.ListSelectedItem != null;
            }
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainWindowModel model)
            {
                e.CanExecute = model.ListSelectedItem != null;
            }
        }

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Rename_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainWindowModel model)
            {
                e.CanExecute = model.ListSelectedItem != null;
            }
        }

        private void Rename_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainWindowModel model)
            {
                e.CanExecute = model.ListItems.Count > 0;
            }
        }

        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            fileListView.SelectAll();
        }

        private void SortMode_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source != null && e.Source is MenuItem item && item.CommandParameter != null
                && DataContext is ViewModels.MainWindowModel model)
            {
                var parameter = item.CommandParameter.ToString();
                if (Enum.TryParse(parameter.ToString(), out Common.SortModes sm))
                {
                    model.SortMode = sm;
                }
            }
        }

        private void AscDesc_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source != null && e.Source is MenuItem item && item.CommandParameter != null 
                && DataContext is ViewModels.MainWindowModel model)
            {
                model.SortAscending = $"{item.CommandParameter}" == "Asc";
            }
        }

        private void ViewMode_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source != null && e.Source is MenuItem item && item.CommandParameter != null
                && DataContext is ViewModels.MainWindowModel model)
            {
                var parameter = item.CommandParameter.ToString();
                if (Enum.TryParse(parameter, out Common.FileViewModes vm))
                {
                    model.ViewMode = vm;
                }
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ViewModels.MainWindowModel model)
                model.OnListDblClick(sender, e);
        }
    }
}
