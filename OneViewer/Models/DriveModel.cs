using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace OneViewer.Models
{
    internal class DriveModel : DirectoryModel
    {
        #region Properties
        public DriveType InfoDriveType { get; private set; }
        public string VolumeLabel { get; private set; }
        public string DriveFormat { get; private set; }
        public long TotalSize { get; private set; }
        public long TotalFreeSpace { get; private set; }
        public long AvailableFreeSpace { get; private set; }
        public DirectoryInfo RootDirectory { get; private set; }
        public bool IsReady { get; private set; }
        #endregion

        #region Constructor
        public DriveModel()
            : base()
        {
        }

        public DriveModel(DriveInfo info)
            : base(info.RootDirectory)
        {
            switch(info.DriveType)
            {
                case DriveType.Removable:
                    FileSystemType = Common.FileSystemTypes.Drive | Common.FileSystemTypes.USBDrive;
                    break;
                case DriveType.Fixed:
                    FileSystemType = Common.FileSystemTypes.Drive | Common.FileSystemTypes.HardDrive;
                    break;
                case DriveType.Network:
                    FileSystemType = Common.FileSystemTypes.Drive | Common.FileSystemTypes.NetworkDrive;
                    break;
                case DriveType.CDRom:
                    FileSystemType = Common.FileSystemTypes.Drive | Common.FileSystemTypes.OpticalDrive;
                    break;
                case DriveType.Ram:
                    FileSystemType = Common.FileSystemTypes.Drive | Common.FileSystemTypes.SDCard;
                    break;
                default:
                    FileSystemType = Common.FileSystemTypes.Drive;
                    break;
            }

            InfoDriveType = info.DriveType;
            VolumeLabel = info.VolumeLabel;
            DriveFormat = info.DriveFormat;
            TotalSize = info.TotalSize;
            TotalFreeSpace = info.TotalFreeSpace;
            AvailableFreeSpace = info.AvailableFreeSpace;
            RootDirectory = info.RootDirectory;
            IsReady = info.IsReady;
            
            if (!string.IsNullOrEmpty(info.VolumeLabel))
                Name = $"{info.VolumeLabel} ({info.Name})";
            else if (info.DriveType == DriveType.Fixed)
                Name = $"Local Disk ({info.Name})";
            else
                Name = info.Name;

            SetSize(info.TotalSize);
        }
        #endregion

        #region Methods
        public static IEnumerable<DriveModel> LoadDrives()
        {
            // Environment.GetLogicalDrives()

            var drives = DriveInfo.GetDrives();
            return drives.Select(x => new DriveModel(x));
        }
        #endregion
    }
}
