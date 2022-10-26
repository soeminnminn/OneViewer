using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneViewer.Models
{
    internal class DirectoryModel : FileSystemObjectModel
    {
        #region Properties
        public FileAttributes Attributes { get; protected set; }

        private IList<DirectoryModel> mDirectories = null;
        public IList<DirectoryModel> Directories
        {
            get => mDirectories;
            private set { SetProperty(ref mDirectories, value); }
        }

        private IList<FileModel> mFiles = null;
        public IList<FileModel> Files
        {
            get => mFiles;
            private set { SetProperty(ref mFiles, value); }
        }

        public IEnumerable<FileSystemObjectModel> Children
        {
            get
            {
                var children = new List<FileSystemObjectModel>();
                if (mDirectories != null)
                    children.AddRange(mDirectories);
                if (mFiles != null)
                    children.AddRange(mFiles);

                return children;
            }
        }
        #endregion

        #region Constructor
        public DirectoryModel()
        { }

        public DirectoryModel(DirectoryInfo info)
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

            IsHidden = (info.Attributes & FileAttributes.Hidden) != 0;
            IsSystem = (info.Attributes & FileAttributes.System) != 0;
        }
        #endregion

        #region Methods
        #endregion
    }
}
