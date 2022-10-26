using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using OneViewer.Common;

namespace OneViewer.Models
{
    public class DriveModel : DirectoryModel
    {
        #region Variables
        #endregion

        #region Properties
        public DriveType InfoDriveType { get; private set; }
        public string VolumeLabel { get; private set; }
        public string DriveFormat { get; private set; }
        public long TotalSize { get; private set; }
        public long TotalFreeSpace { get; private set; }
        public long AvailableFreeSpace { get; private set; }
        public DirectoryInfo RootDirectory { get => directoryInfo; }
        public bool IsReady { get; private set; }
        #endregion

        #region Constructor
        public DriveModel()
            : base()
        {
            TypeName = "Local disk";
            FileSystemType = FileSystemTypes.Drive;
        }

        public DriveModel(DriveInfo info)
            : base(info.RootDirectory)
        {
            switch (info.DriveType)
            {
                case DriveType.Removable:
                    FileSystemType = FileSystemTypes.Drive | FileSystemTypes.USBDrive;
                    break;
                case DriveType.Fixed:
                    FileSystemType = FileSystemTypes.Drive | FileSystemTypes.HardDrive;
                    break;
                case DriveType.Network:
                    FileSystemType = FileSystemTypes.Drive | FileSystemTypes.NetworkDrive;
                    break;
                case DriveType.CDRom:
                    FileSystemType = FileSystemTypes.Drive | FileSystemTypes.OpticalDrive;
                    break;
                case DriveType.Ram:
                    FileSystemType = FileSystemTypes.Drive | FileSystemTypes.SDCard;
                    break;
                default:
                    FileSystemType = FileSystemTypes.Drive;
                    break;
            }

            InfoDriveType = info.DriveType;
            VolumeLabel = info.VolumeLabel;
            DriveFormat = info.DriveFormat;
            TotalSize = info.TotalSize;
            TotalFreeSpace = info.TotalFreeSpace;
            AvailableFreeSpace = info.AvailableFreeSpace;
            IsReady = info.IsReady;

            var name = info.Name.Replace("\\", "");

            if (!string.IsNullOrEmpty(info.VolumeLabel))
                Name = $"{info.VolumeLabel} ({name})";
            else if (info.DriveType == DriveType.Fixed)
                Name = $"Local Disk ({name})";
            else
                Name = info.Name;

            ShortName = name;
            TypeName = "Local disk";

            SetSize(info.TotalSize);
        }
        #endregion

        #region Methods
        #endregion
    }
}
