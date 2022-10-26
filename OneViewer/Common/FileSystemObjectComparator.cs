using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;
using OneViewer.Models;

namespace OneViewer.Common
{
    public class FileSystemObjectComparator : IComparer<FileSystemObjectModel>, IEqualityComparer<FileSystemObjectModel>
    {
        private SortModes mSortMode = SortModes.Name;
        private bool mSortAsc = true;

        public SortModes SortMode
        {
            get => mSortMode;
            set { mSortMode = value; }
        }

        public bool IsAscending
        {
            get => mSortAsc;
            set { mSortAsc = value; }
        }

        public FileSystemObjectComparator()
        { }

        public FileSystemObjectComparator(SortModes sortMode, bool asc = true)
        {
            mSortMode = sortMode;
            mSortAsc = asc;
        }

        public int Compare(FileSystemObjectModel x, FileSystemObjectModel y)
        {
            var one = new Holder(x);
            var two = new Holder(y);

            int result;
            if (one.IsNull && two.IsNull)
                return 0;
            else if (!one.IsNull && two.IsNull)
                return -1;
            else if (one.IsNull && !two.IsNull)
                return 1;
            else if (one.IsDirectory && !two.IsDirectory)
                return -1;
            else if (!one.IsDirectory && two.IsDirectory)
                return 1;
            else
            {
                if (mSortMode == SortModes.Name)
                    result = string.Compare(one.Name, two.Name, true);
                else
                {
                    long val1;
                    long val2;

                    if (mSortMode == SortModes.Modified)
                    {
                        val1 = one.LastModified;
                        val2 = two.LastModified;
                    }
                    else if (mSortMode == SortModes.DateTaken)
                    {
                        val1 = one.DateTaken;
                        val2 = two.DateTaken;
                    }
                    else
                    {
                        val1 = one.Length;
                        val2 = two.Length;
                    }

                    if (val1 > val2)
                        result = 1;
                    else if (val1 < val2)
                        result = -1;
                    else
                        result = 0;
                }
            }

            if (result == 0) return 0;
            if (!IsAscending) return -result;
            return result;
        }

        public bool Equals(FileSystemObjectModel x, FileSystemObjectModel y)
        {
            var one = new Holder(x);
            var two = new Holder(y);

            if (one.IsNull && two.IsNull)
                return true;
            else if (one.IsNull || two.IsNull)
                return false;
            else if (one.IsDirectory && two.IsDirectory)
                return one.Path == two.Path;
            else if (one.IsDirectory || two.IsDirectory)
                return false;

            return one.Path == two.Path;
        }

        public int GetHashCode([DisallowNull] FileSystemObjectModel obj)
        {
            return obj.GetHashCode();
        }

        private record Holder(FileSystemObjectModel src)
        {
            public bool IsNull { get => src == null; }

            public bool IsDirectory { get => (src.FileSystemType & FileSystemTypes.Folder) == FileSystemTypes.Folder; }

            public long LastModified { get => src.LastAccessTime.ToFileTime(); }

            public long DateTaken { get => src.CreationTime.ToFileTime(); }

            public long Length { get => src.Size; }

            public string Name { get => src.Name; }

            public string Path { get => src.FullName; }
        }
    }
}
