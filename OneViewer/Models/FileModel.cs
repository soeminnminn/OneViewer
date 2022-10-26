using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneViewer.Models
{
    public class FileModel : FileSystemObjectModel 
    {
        #region Properties
        public FileAttributes Attributes { get; protected set; }
        public string DirectoryName { get; protected set; }
        public bool IsReadOnly { get; protected set; }
        public long Length { get; protected set; }
        public override DirectoryModel Parent
        {
            get
            {
                var info = new FileInfo(FullName);
                if (info.Exists && info.Directory != null)
                    return new DirectoryModel(info.Directory);

                return null;
            }
        }
        #endregion

        #region Constructor
        public FileModel()
        {
            TypeName = "File";
        }

        public FileModel(FileInfo info)
            : this()
        {
            Attributes = info.Attributes;
            CreationTime = info.CreationTime;
            DirectoryName = info.DirectoryName;
            Exists = info.Exists;
            Extension = info.Extension;
            FullName = info.FullName;
            IsReadOnly = info.IsReadOnly;
            LastAccessTime = info.LastAccessTime;
            LastWriteTime = info.LastWriteTime;
            Length = info.Length;
            LinkTarget = info.LinkTarget;
            Name = info.Name;

            ShortName = Path.GetFileNameWithoutExtension(info.FullName);

            SetSize(info.Length);

            IsHidden = (info.Attributes & FileAttributes.Hidden) != 0;
            IsSystem = (info.Attributes & FileAttributes.System) != 0;
        }
        #endregion
    }
}
