using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using OneViewer.Common;
using System.Runtime.InteropServices;

namespace OneViewer.Models
{
    internal class TreeNodeModel : FileSystemObjectModel
    {
        #region Properties
        private bool mIsExpanded = false;
        public bool IsExpanded 
        {
            get => mIsExpanded;
            set { SetProperty(ref mIsExpanded, value); }
        }

        public string Label 
        { 
            get
            {
                if (IsRoot) return "Root";
                else if (IsHome) return "Library";
                else if (IsComputer) return "Computer";
                else return Name;
            }
        }

        public ObservableCollection<TreeNodeModel> Children { get; }

        public bool IsRoot { get => (FileSystemType & FileSystemTypes.Root) == FileSystemTypes.Root; }

        public bool IsHome { get => (FileSystemType & FileSystemTypes.Home) == FileSystemTypes.Home; }

        public bool IsDesktop { get; private set; }

        public bool IsDocuments { get; private set; }

        public bool IsDownloads { get; private set; }

        public bool IsMyMusic { get; private set; }

        public bool IsMyPicture { get; private set; }

        public bool IsMyVideo { get; private set; }

        public bool IsComputer { get => (FileSystemType & FileSystemTypes.Computer) == FileSystemTypes.Computer; }

        public bool IsHardDrive { get => (FileSystemType & FileSystemTypes.HardDrive) == FileSystemTypes.HardDrive; }

        public bool IsOpticalDrive { get => (FileSystemType & FileSystemTypes.OpticalDrive) == FileSystemTypes.OpticalDrive; }

        public bool IsSDCard { get => (FileSystemType & FileSystemTypes.SDCard) == FileSystemTypes.SDCard; }

        public bool IsUSBDrive { get => (FileSystemType & FileSystemTypes.USBDrive) == FileSystemTypes.USBDrive; }

        public bool IsPhoneDrive { get => (FileSystemType & FileSystemTypes.PhoneDrive) == FileSystemTypes.PhoneDrive; }

        public bool IsNetworkDrive { get => (FileSystemType & FileSystemTypes.NetworkDrive) == FileSystemTypes.NetworkDrive; }

        public bool IsFolder { get; private set; }

        public bool IsFavoriteFolder { get => (FileSystemType & FileSystemTypes.FavoriteFolder) == FileSystemTypes.FavoriteFolder; }

        public bool IsImageFile { get => (FileSystemType & FileSystemTypes.ImageFile) == FileSystemTypes.ImageFile; }

        public bool IsVideoFile { get => (FileSystemType & FileSystemTypes.VideoFile) == FileSystemTypes.VideoFile; }

        public bool IsAudioFile { get => (FileSystemType & FileSystemTypes.AudioFile) == FileSystemTypes.AudioFile; }

        public bool IsGenericFile { get => (FileSystemType & FileSystemTypes.GenericFile) == FileSystemTypes.GenericFile; }
        #endregion

        #region Constructor
        public TreeNodeModel()
            : this(FileSystemTypes.Unknown, LibraryTypes.None)
        {
        }

        public TreeNodeModel(FileSystemTypes systemType, LibraryTypes libraryType = LibraryTypes.None)
            : base()
        {
            string dirPath = string.Empty;
            string name = string.Empty;
            FileSystemTypes sysType = FileSystemTypes.Unknown;

            if (systemType == FileSystemTypes.Root)
            {
                name = "Root";
                sysType = FileSystemTypes.Root;
                
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    dirPath = "/";
            }
            else if (systemType == FileSystemTypes.Home)
            {
                name = "Home";
                sysType = FileSystemTypes.Home;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    name = "Library";

                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else if (systemType == FileSystemTypes.Computer)
            {
                name = "Computer";
                sysType = FileSystemTypes.Computer;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    dirPath = "/";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            }
            else if (libraryType == LibraryTypes.Desktop)
            {
                name = "Desktop";
                IsDesktop = true;

                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            else if (libraryType == LibraryTypes.Documents)
            {
                name = "Documents";
                IsDocuments = true;

                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else if (libraryType == LibraryTypes.Downloads)
            {
                name = "Downloads";
                IsDownloads = true;

                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                dirPath = Path.Combine(dirPath, "Downloads");
            }
            else if (libraryType == LibraryTypes.Music)
            {
                name = "Music";
                IsMyMusic = true;

                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }
            else if (libraryType == LibraryTypes.Pictures)
            {
                name = "Pictures";
                IsMyPicture = true;

                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            else if (libraryType == LibraryTypes.Videos)
            {
                name = "Videos";
                IsMyVideo = true;

                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            }

            if (!string.IsNullOrEmpty(dirPath))
            {
                var dirInfo = new DirectoryInfo(dirPath);
                var src = new DirectoryModel(dirInfo);
                src.CopyTo(this);
            }

            Name = name;
            FileSystemType = sysType;

            if (systemType == FileSystemTypes.Root)
            {
                var list = new List<TreeNodeModel>();
                list.Add(HomeFolder);
                list.Add(Computer);

                Children = new ObservableCollection<TreeNodeModel>(list);
            }
            else if (systemType == FileSystemTypes.Home)
            {
                var list = new List<TreeNodeModel>();
                list.Add(Documents);
                list.Add(Downloads);
                list.Add(Music);
                list.Add(Pictures);
                list.Add(Videos);

                Children = new ObservableCollection<TreeNodeModel>(list.TakeWhile(x => x.Exists));
                IsExpanded = true;
            }
            else if (systemType == FileSystemTypes.Computer)
            {
                var list = new List<TreeNodeModel>(DriveModel.LoadDrives().Select(x => new TreeNodeModel(x)));
                Children = new ObservableCollection<TreeNodeModel>(list);
                IsExpanded = true;
            }
            else
            {
                this.Children = new ObservableCollection<TreeNodeModel>();
            }
        }

        public TreeNodeModel(FileSystemObjectModel src)
            : this()
        {
            if (src != null)
            {
                src.CopyTo(this);
            }
        }
        #endregion

        #region Static Properties
        public static TreeNodeModel Root
        {
            get => new TreeNodeModel(FileSystemTypes.Root);
        }

        public static TreeNodeModel HomeFolder
        {
            get => new TreeNodeModel(FileSystemTypes.Home);
        }

        public static TreeNodeModel Computer
        {
            get => new TreeNodeModel(FileSystemTypes.Computer);
        }

        public static TreeNodeModel Documents
        {
            get => new TreeNodeModel(FileSystemTypes.Folder, LibraryTypes.Documents);
        }

        public static TreeNodeModel Downloads
        {
            get => new TreeNodeModel(FileSystemTypes.Folder, LibraryTypes.Downloads);
        }

        public static TreeNodeModel Music
        {
            get => new TreeNodeModel(FileSystemTypes.Folder, LibraryTypes.Music);
        }

        public static TreeNodeModel Pictures
        {
            get => new TreeNodeModel(FileSystemTypes.Folder, LibraryTypes.Pictures);
        }

        public static TreeNodeModel Videos
        {
            get => new TreeNodeModel(FileSystemTypes.Folder, LibraryTypes.Videos);
        }
        #endregion

        #region Methods
        #endregion

        #region Nested Types
        #endregion
    }
}
