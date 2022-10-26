using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using OneViewer.Models;
using OneViewer.Common;
using OneViewer.Observable;

namespace OneViewer.ViewModels
{
    public class MainWindowModel : ObservableObject
    {
        #region Variables
        private FileSystemObjectComparator comparator;
        #endregion

        #region Constructors
        public MainWindowModel()
        {
            foreach(var item in Factories.FileSystemFactory.GetLibraryForders())
            {
                LibraryItems.Add(item);
            }
            
            foreach(var item in Factories.FileSystemFactory.GetDrives())
            {
                Drives.Add(item);
            }

            comparator = new FileSystemObjectComparator();
        }
        #endregion

        #region Properties
        public ObservableCollection<DirectoryModel> LibraryItems { get; } = new ObservableCollection<DirectoryModel>();
        public ObservableCollection<DirectoryModel> Drives { get; } = new ObservableCollection<DirectoryModel>();

        public ObservableCollection<DirectoryModel> FavouriteItems { get; } = new ObservableCollection<DirectoryModel>();

        public ObservableCollection<DirectoryModel> BreadcrumbItems { get; } = new ObservableCollection<DirectoryModel>();

        public SortableObservableCollection<FileSystemObjectModel> ListItems { get; } = new SortableObservableCollection<FileSystemObjectModel>();

        private DirectoryModel favSelectedItem = null;
        public DirectoryModel FavSelectedItem
        {
            get => favSelectedItem;
            set { SetProperty(ref favSelectedItem, value, nameof(FavSelectedItem), OnFavSelectedItemChanged); }
        }
        
        private DirectoryModel libSelectedItem = null;
        public DirectoryModel LibSelectedItem
        {
            get => libSelectedItem;
            set { SetProperty(ref libSelectedItem, value, nameof(LibSelectedItem), OnLibSelectedItemChanged); }
        }

        private DirectoryModel driveSelectedItem = null;
        public DirectoryModel DriveSelectedItem
        {
            get => driveSelectedItem;
            set { SetProperty(ref driveSelectedItem, value, nameof(DriveSelectedItem), OnDriveSelectedItemChanged); }
        }

        private DirectoryModel breadcrumbSelectedItem = null;
        public DirectoryModel BreadcrumbSelectedItem
        {
            get => breadcrumbSelectedItem;
            set { SetProperty(ref breadcrumbSelectedItem, value, nameof(BreadcrumbSelectedItem), OnBreadcrumbSelectedItemChanged); }
        }

        private FileSystemObjectModel listSelectedItem = null;
        public FileSystemObjectModel ListSelectedItem
        {
            get => listSelectedItem;
            set { SetProperty(ref listSelectedItem, value); }
        }

        private DirectoryModel currentDirectory = null;
        public DirectoryModel CurrentDirectory
        {
            get => currentDirectory;
            set { SetProperty(ref currentDirectory, value, nameof(CurrentDirectory), OnCurrentDirectoryChanged); }
        }

        private bool showInfo = false;
        public bool IsShowInfo
        {
            get => showInfo;
            set { SetProperty(ref showInfo, value); }
        }

        private FileViewModes viewMode = FileViewModes.Tiles;
        public FileViewModes ViewMode
        {
            get => viewMode;
            set { SetProperty(ref viewMode, value); }
        }

        private SortModes sortMode = SortModes.Name;
        public SortModes SortMode
        {
            get => sortMode;
            set { SetProperty(ref sortMode, value, nameof(SortMode), OnSortModeChanged); }
        }

        private bool sortAscending = true;
        public bool SortAscending
        {
            get => sortAscending;
            set { SetProperty(ref sortAscending, value, nameof(SortAscending), OnSortModeChanged); }
        }
        #endregion

        #region Methods
        private void OnFavSelectedItemChanged()
        {
            if (favSelectedItem != null)
            {
                DriveSelectedItem = null;
                LibSelectedItem = null;

                OnNavSelectedItemChanged(favSelectedItem);
            }
        }

        private void OnLibSelectedItemChanged()
        {
            if (libSelectedItem != null)
            {
                DriveSelectedItem = null;
                FavSelectedItem = null;

                OnNavSelectedItemChanged(libSelectedItem);
            }
        }

        private void OnDriveSelectedItemChanged()
        {
            if (driveSelectedItem != null)
            {
                LibSelectedItem = null;
                FavSelectedItem = null;

                OnNavSelectedItemChanged(driveSelectedItem);
            }
        }

        private void OnCurrentDirectoryChanged()
        {
            UpdateList(currentDirectory);
            UpadteBreadcrumb(currentDirectory);
        }

        private void OnNavSelectedItemChanged(DirectoryModel item)
        {
            if (currentDirectory != item)
                CurrentDirectory = item;
        }

        private void OnBreadcrumbSelectedItemChanged()
        {
            if (breadcrumbSelectedItem != null && currentDirectory != breadcrumbSelectedItem)
                CurrentDirectory = breadcrumbSelectedItem;
        }

        public void OnListDblClick(object sender, MouseButtonEventArgs e)
        {
            if (listSelectedItem != null && listSelectedItem is DirectoryModel dirItem && dirItem != currentDirectory)
            {
                CurrentDirectory = dirItem;
            }
        }

        private void OnSortModeChanged()
        {
            comparator.SortMode = sortMode;
            comparator.IsAscending = sortAscending;
            ListItems.Sort(comparator);
        }

        private void UpdateList(DirectoryModel source)
        {
            ListItems.Clear();

            if (source == null) return;
            foreach (var item in source.GetChildren())
            {
                ListItems.Add(item);
            }
        }

        private void UpadteBreadcrumb(DirectoryModel source)
        {
            BreadcrumbItems.Clear();
            if (source == null) return;

            var list = new List<DirectoryModel>();
            list.Add(source);

            var parent = source.Parent;
            while (parent != null)
            {
                list.Add(parent);
                parent = parent.Parent;
            }

            list.Reverse();
            
            foreach(var item in list)
            {
                BreadcrumbItems.Add(item);
            }
        }
        #endregion
    }
}