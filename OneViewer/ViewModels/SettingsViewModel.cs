using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneViewer.ViewModels
{
    public class SettingsViewModel : Observable.ObservableObject
    {
        public ObservableCollection<string> TestStrings { get; } = new ObservableCollection<string>(new string[]
            {
                "Sample data 1",
                "Sample data 2",
                "Sample data 3",
                "Sample data 4",
                "Sample data 5",
                "Sample data 6",
                "Sample data 7",
                "Sample data 8",
                "Sample data 9"
            });

        private string selectedItem = null;
        public string SelectedItem
        {
            get => selectedItem;
            set { SetProperty(ref selectedItem, value, nameof(SelectedItem), OnSelectedItemChanged); }
        }

        public SettingsViewModel()
        {
        }

        private void OnSelectedItemChanged()
        {

        }
    }
}
