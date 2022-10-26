using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace OneViewer.Common
{
    internal class IconsManager
    {
        private static readonly string DefaultIconThemeName = "Win10Sur";
        private static DirectoryInfo IconThemesDir
        {
            get
            {
                var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
                return new DirectoryInfo(Path.Combine(applicationDirectory, "Resources", "Icons"));
            }
        }

        private static IconTheme LoadIndexTheme(FileInfo zipFile)
        {
            if (zipFile.Exists)
            {
                try
                {
                    using (var zip = new ZipFile(zipFile.OpenRead()))
                    {
                        var id = zip.FindEntry("index.theme", true);
                        if (id > -1)
                        {
                            using (var zipStream = zip.GetInputStream(id))
                            {
                                var conf = SharpConfig.Configuration.LoadFromStream(zipStream, Encoding.UTF8);

                                var list = new List<string>();
                                foreach(ZipEntry ze in zip)
                                {
                                    if (ze.IsDirectory) continue;
                                    list.Add(ze.Name);
                                }

                                var iconTheme = IconTheme.From(conf, zipFile.FullName);
                                iconTheme.SetEntities(list);
                                return iconTheme;
                            }
                        }
                    }
                }
                catch (Exception)
                { }
            }
            return null;
        }

        private static IconsManager mInstance = new IconsManager();
        public static IconsManager Instance
        {
            get => mInstance;
        }

        private IconTheme currentTheme = null;

        private Dictionary<string, IconTheme> mThemes = new Dictionary<string, IconTheme>();

        public IconsManager()
        {
        }

        public bool TryLoad(string themeName = "")
        {
            if (mThemes.Count == 0)
            {
                var dir = IconThemesDir;
                if (!dir.Exists) return false;

                var files = dir.EnumerateFiles("*.zip");
                if (files == null) return false;

                var themes = files.Select(x => LoadIndexTheme(x)).TakeWhile(x => x != null);
                if (themes == null) return false;

                mThemes = themes.ToDictionary(x => x.Name, x => x);
            }

            string name = string.IsNullOrEmpty(themeName) ? DefaultIconThemeName : themeName;
            if (mThemes.TryGetValue(name, out IconTheme it))
            {
                currentTheme = it;
                return true;
            }

            return false;
        }

        #region Nested Types
        private class IconTheme
        {
            private Dictionary<string, object> sectionData = new Dictionary<string, object>();

            public string Name { get; set; } = string.Empty;
            public string Comment { get; set; } = string.Empty;
            public string Inherits { get; set; } = string.Empty;
            public string Example { get; set; } = string.Empty;
            public ThemeDirectory[] Directories { get; private set; }

            public string FilePath { get; private set; }

            public string[] Entities { get; private set; }

            public static IconTheme From(SharpConfig.Configuration indexTheme, string filePath)
            {
                var data = new IconTheme()
                {
                    FilePath = filePath
                };

                var cfgData = indexTheme["Icon Theme"];
                if (cfgData != null)
                {
                    cfgData.SetValuesTo(data);

                    var directories = cfgData["Directories"];
                    if (directories != null)
                    {
                        data.Directories = directories.StringValue.Split(',').Select(x => ThemeDirectory.From(indexTheme[x], x)).ToArray();
                    }

                    data.sectionData = cfgData.ToDictionary(x => x.Name, x =>
                    {
                        var str = x.RawValue;

                        object value = null;
                        if (!string.IsNullOrEmpty(str))
                        {
                            var splited = str.Split(',');
                            if (splited != null && splited.Length > 1)
                            {
                                value = splited;
                            }
                        }

                        if (value == null)
                            value = str;

                        return value;
                    });
                }

                return data;
            }

            public bool TryGetValue(string key, out object value) => sectionData.TryGetValue(key, out value);

            public void SetEntities(IEnumerable<string> list)
            {
                if (list != null)
                    Entities = list.ToArray();
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private class ThemeDirectory
        {
            public string Name { get; set; }
            public int Size { get; set; }
            public string Context { get; set; }
            public int MinSize { get; set; }
            public int MaxSize { get; set; }
            public string Type { get; set; }

            public static ThemeDirectory From(SharpConfig.Section cfgData, string name)
            {
                var data = new ThemeDirectory();
                cfgData.SetValuesTo(data);
                data.Name = name;
                return data;
            }

            public override string ToString()
            {
                return $"{Name} (Context = {Context}, Size {Size})";
            }
        }
        #endregion
    }
}
