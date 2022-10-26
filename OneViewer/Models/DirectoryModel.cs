using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using OneViewer.Common;
using static System.Net.WebRequestMethods;

namespace OneViewer.Models
{
    public class DirectoryModel : FileSystemObjectModel
    {
        #region Variables
        protected DirectoryInfo directoryInfo = null;
        #endregion

        #region Properties
        public FileAttributes Attributes { get; protected set; }

        public LibraryTypes LibraryType { get; protected set; }

        public override string SizeString
        {
            get => string.Empty;
        }

        public override DirectoryModel Parent
        {
            get
            {
                var info = new DirectoryInfo(FullName);
                if (info.Exists && info.Parent != null)
                    return new DirectoryModel(info.Parent);

                return null;
            }
        }
        #endregion

        #region Constructor
        protected DirectoryModel()
        {
            FileSystemType = FileSystemTypes.Folder;
            TypeName = "File folder";
        }

        public DirectoryModel(string path)
            : this()
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                directoryInfo = new DirectoryInfo(path);
                LoadInfo(directoryInfo);

                ShortName = directoryInfo.Name;
            }
        }

        public DirectoryModel(LibraryTypes libraryType)
            : this()
        {
            LibraryType = libraryType;

            string dirPath = string.Empty;
            string name = string.Empty;
            if (libraryType == LibraryTypes.Desktop)
            {
                name = "Desktop";
                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                FileSystemType |= FileSystemTypes.Desktop;
            }
            else if (libraryType == LibraryTypes.Documents)
            {
                name = "Documents";
                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else if (libraryType == LibraryTypes.Downloads)
            {
                name = "Downloads";
                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                dirPath = Path.Combine(dirPath, "Downloads");
            }
            else if (libraryType == LibraryTypes.Music)
            {
                name = "Music";
                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }
            else if (libraryType == LibraryTypes.Pictures)
            {
                name = "Pictures";
                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            else if (libraryType == LibraryTypes.Videos)
            {
                name = "Videos";
                dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            }

            if (!string.IsNullOrEmpty(dirPath))
            {
                var info = new DirectoryInfo(dirPath);
                if (info.Exists)
                {
                    directoryInfo = info;
                    LoadInfo(directoryInfo);
                    ShortName = name;
                }
            }
        }

        public DirectoryModel(DirectoryInfo info)
            : this()
        {
            directoryInfo = info;
            LoadInfo(info);

            if (info.Parent == null)
                ShortName = info.Name.Replace("\\", "");
            else
                ShortName = info.Name;
        }
        #endregion

        #region Methods
        private void LoadInfo(DirectoryInfo info)
        {
            Attributes = info.Attributes;
            CreationTime = info.CreationTime;
            Exists = info.Exists;
            Extension = info.Extension;
            FullName = info.FullName;
            LastAccessTime = info.LastAccessTime;
            LastWriteTime = info.LastWriteTime;
            LinkTarget = info.LinkTarget;
            Name = info.Name;
            Exists = info.Exists;

            IsHidden = (info.Attributes & FileAttributes.Hidden) != 0;
            IsSystem = (info.Attributes & FileAttributes.System) != 0;
        }

        public IEnumerable<FileSystemObjectModel> GetChildren()
        {
            if (directoryInfo.Exists)
            {
                var dirs = directoryInfo.GetDirectories();
                foreach(var dir in dirs)
                {
                    if (dir.Attributes.HasFlag(FileAttributes.System)) continue;
                    yield return new DirectoryModel(dir);
                }

                var files = directoryInfo.GetFiles();
                foreach(var file in files)
                {
                    if (file.Attributes.HasFlag(FileAttributes.System)) continue;
                    yield return new FileModel(file);
                }
            }
        }
        #endregion
    }
}
