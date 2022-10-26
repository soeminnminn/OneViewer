using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Resources;

namespace OneViewer.Factories
{
    internal static class IconsDataFactory
    {
        private const string iconResPath = "themes/pathicons.baml";

        internal static IDictionary<string, Geometry> Create()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourcesName = assembly.GetName().Name + ".g";
            var manager = new ResourceManager(resourcesName, assembly);
            var resourceSet = manager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            var dictionaryEntries = resourceSet.OfType<DictionaryEntry>().ToList();
            var assemblyName = assembly.GetName().Name;

            var iconRes = dictionaryEntries.FirstOrDefault(x => x.Key.ToString() == iconResPath);
            if (iconRes.Key != null && iconRes.Value != null)
            {
                var path = iconRes.Key.ToString();
                var resDict = (ResourceDictionary)Application.LoadComponent(new Uri(
                    $"/{assemblyName};component/{path.Replace(".baml", ".xaml")}",
                    UriKind.RelativeOrAbsolute));

                return resDict.OfType<DictionaryEntry>()
                    .Where(x => x.Key.ToString().StartsWith("Path") && x.Value is StreamGeometry)
                    .ToDictionary(x => x.Key.ToString(), x => x.Value as Geometry);
            }
            return new Dictionary<string, Geometry>();
        }
    }
}
