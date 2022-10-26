using System;
using System.IO;
using OneViewer.Common;
using OneViewer.Observable;

namespace OneViewer.Models
{
    public abstract class FileSystemObjectModel : ObservableObject, ICloneable
    {
        #region Properties
        private FileSystemTypes mFileSystemType = FileSystemTypes.Unknown;
        public FileSystemTypes FileSystemType 
        {
            get => mFileSystemType;
            protected set { SetProperty(ref mFileSystemType, value); } 
        }

        private string mName = string.Empty;
        public string Name 
        {
            get => mName;
            protected set { SetProperty(ref mName, value); }
        }

        private string mExtension = string.Empty;
        public string Extension 
        {
            get => mExtension;
            protected set { SetProperty(ref mExtension, value); } 
        }

        private string mFullName = string.Empty;
        public string FullName
        {
            get => mFullName;
            protected set { SetProperty(ref mFullName, value); }
        }

        private long mSize = 0L;
        public string SizeString
        {
            get
            {
                if ((FileSystemType & FileSystemTypes.Folder) != 0)
                    return string.Empty;
                return Constants.FormatFileSize(mSize, 2);
            }
        }

        private DateTime mCreationTime = DateTime.MinValue;
        public DateTime CreationTime 
        {
            get => mCreationTime;
            protected set { SetProperty(ref mCreationTime, value); } 
        }

        private DateTime mLastAccessTime = DateTime.MinValue;
        public DateTime LastAccessTime 
        {
            get => mLastAccessTime;
            protected set { SetProperty(ref mLastAccessTime, value); } 
        }

        private DateTime mLastWriteTime = DateTime.MinValue;
        public DateTime LastWriteTime 
        {
            get => mLastWriteTime;
            protected set { SetProperty(ref mLastWriteTime, value); }
        }

        private bool mIsHidden = false;
        public bool IsHidden
        {
            get => mIsHidden;
            protected set { SetProperty(ref mIsHidden, value); }
        }

        private bool mIsSystem = false;
        public bool IsSystem
        {
            get => mIsSystem;
            protected set { SetProperty(ref mIsSystem, value); }
        }

        private bool mExists = false;
        public bool Exists 
        {
            get => mExists;
            protected set { SetProperty(ref mExists, value); }
        }

        private string mLinkTarget = string.Empty;
        public string LinkTarget 
        {
            get => mLinkTarget;
            protected set { SetProperty(ref mLinkTarget, value); }
        }
        #endregion

        #region Constructor
        public FileSystemObjectModel()
        { }
        #endregion

        #region Methods
        public void SetSize(long size)
        {
            mSize = size;
            OnPropertyChanged(nameof(SizeString));
        }

        public string GetIconPath()
        {
            return string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void CopyTo(object other)
        {
            if (other == null) return;
            if (other is FileSystemObjectModel obj)
            {
                obj.FileSystemType = this.mFileSystemType;
                obj.Name = this.mName;
                obj.FullName = this.mFullName;
                obj.Extension = this.mExtension;
                obj.SetSize(this.mSize);
                obj.IsHidden = this.mIsHidden;
                obj.IsSystem = this.mIsSystem;
                obj.Exists = this.mExists;
                obj.LinkTarget = this.mLinkTarget;
                obj.CreationTime = this.mCreationTime;
                obj.LastAccessTime = this.mLastAccessTime;
                obj.LastWriteTime = this.mLastWriteTime;
            }
        }
        #endregion
    }
}
