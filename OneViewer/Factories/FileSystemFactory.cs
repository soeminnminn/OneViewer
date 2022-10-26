using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OneViewer.Models;

namespace OneViewer.Factories
{
    internal class FileSystemFactory
    {
        public static IEnumerable<DriveModel> GetDrives()
        {
            var drives = DriveInfo.GetDrives();
            foreach(var drive in drives)
            {
                yield return new DriveModel(drive);
            }
        }

        public static IEnumerable<DirectoryModel> GetLibraryForders()
        {
            var list = new List<DirectoryModel>();
            list.Add(new DirectoryModel(Common.LibraryTypes.Desktop));
            list.Add(new DirectoryModel(Common.LibraryTypes.Documents));
            list.Add(new DirectoryModel(Common.LibraryTypes.Downloads));
            list.Add(new DirectoryModel(Common.LibraryTypes.Music));
            list.Add(new DirectoryModel(Common.LibraryTypes.Pictures));
            list.Add(new DirectoryModel(Common.LibraryTypes.Videos));

            return list.FindAll(x => x.Exists);
        }
    }
}
