using System;

namespace OneViewer.Common
{
    [Flags]
    public enum FileSystemTypes : int
    {
        Unknown = 0x0,
        Root = 0x1,
        Home = 0x2,
        Computer = 0x4,
        Desktop = 0x8,
        // Drive
        Drive = 0x10,
        HardDrive = 0x20,
        OpticalDrive = 0x40,
        SDCard = 0x80,
        USBDrive = 0x100,
        RemovableDrive = 0x200,
        PhoneDrive= 0x400,
        NetworkDrive = 0x800,
        // Folder
        Folder = 0x1000,
        FavoriteFolder = 0x2000,
        // File
        File = 0x4000,
        ImageFile = 0x8000,
        VideoFile = 0x10000,
        AudioFile = 0x20000,
        GenericFile = 0x40000
    }

    public enum LibraryTypes
    {
        None,
        Desktop,
        Documents,
        Downloads,
        Music,
        Pictures,
        Videos
    }

    public enum FileViewModes
    {
        List,
        CompactList,
        Tiles
    }

    public enum SortModes
    {
        Name,
        Modified,
        DateTaken,
        FileSize
    }
}
