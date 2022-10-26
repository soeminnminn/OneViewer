using System;
using System.Globalization;
using System.IO;
using OneViewer.Common;
using OneViewer.Observable;

namespace OneViewer.Models
{
    public abstract class FileSystemObjectModel : ObservableObject, ICloneable
    {
        #region Properties
        private FileSystemTypes mFileSystemType = FileSystemTypes.Unknown;
        public virtual FileSystemTypes FileSystemType 
        {
            get => mFileSystemType;
            protected set { SetProperty(ref mFileSystemType, value); } 
        }

        private string mName = string.Empty;
        public virtual string Name 
        {
            get => mName;
            protected set { SetProperty(ref mName, value); }
        }

        private string mExtension = string.Empty;
        public virtual string Extension 
        {
            get => mExtension;
            protected set { SetProperty(ref mExtension, value); } 
        }

        private string mFullName = string.Empty;
        public virtual string FullName
        {
            get => mFullName;
            protected set { SetProperty(ref mFullName, value); }
        }

        private string mShortName = string.Empty;
        public virtual string ShortName
        {
            get => mShortName;
            protected set { SetProperty(ref mShortName, value); }
        }

        private long mSize = 0L;
        public virtual long Size
        {
            get => mSize;
        }

        public virtual string SizeString
        {
            get
            {
                if ((FileSystemType & FileSystemTypes.Folder) == FileSystemTypes.Folder)
                    return string.Empty;
                return Constants.FormatFileSize(mSize, 2);
            }
        }

        private DateTime mCreationTime = DateTime.MinValue;
        public virtual DateTime CreationTime 
        {
            get => mCreationTime;
            protected set { SetProperty(ref mCreationTime, value); } 
        }

        private DateTime mLastAccessTime = DateTime.MinValue;
        public virtual DateTime LastAccessTime 
        {
            get => mLastAccessTime;
            protected set { SetProperty(ref mLastAccessTime, value); } 
        }

        private DateTime mLastWriteTime = DateTime.MinValue;
        public virtual DateTime LastWriteTime 
        {
            get => mLastWriteTime;
            protected set { SetProperty(ref mLastWriteTime, value); }
        }

        public virtual string DateModified
        {
            get
            {
                if (mLastAccessTime == DateTime.MinValue)
                    return string.Empty;
                return mLastAccessTime.ToString(CultureInfo.CurrentUICulture);
            }
        }

        private bool mIsHidden = false;
        public virtual bool IsHidden
        {
            get => mIsHidden;
            protected set { SetProperty(ref mIsHidden, value); }
        }

        private bool mIsSystem = false;
        public virtual bool IsSystem
        {
            get => mIsSystem;
            protected set { SetProperty(ref mIsSystem, value); }
        }

        private string mTypeName = string.Empty;
        public virtual string TypeName
        {
            get => mTypeName;
            protected set { SetProperty(ref mTypeName, value); }
        }

        private bool mExists = false;
        public virtual bool Exists 
        {
            get => mExists;
            protected set { SetProperty(ref mExists, value); }
        }

        private string mLinkTarget = string.Empty;
        public virtual string LinkTarget 
        {
            get => mLinkTarget;
            protected set { SetProperty(ref mLinkTarget, value); }
        }

        public abstract DirectoryModel Parent { get; }
        #endregion

        #region Constructor
        public FileSystemObjectModel()
        { }
        #endregion

        #region Methods
        public virtual void SetSize(long size)
        {
            mSize = size;
            OnPropertyChanged(nameof(SizeString));
        }

        public virtual string GetIconPath()
        {
            return string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is FileSystemObjectModel om)
            {
                return om.FullName == this.FullName;
            }
            return base.Equals(obj);
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public virtual void CopyTo(object other)
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
