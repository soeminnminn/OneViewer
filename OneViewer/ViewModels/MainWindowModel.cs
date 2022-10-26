using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace OneViewer.ViewModels
{
    internal class MainWindowModel : Observable.ObservableObject
    {
        public SideNavModel SideNav { get; private set; } = new SideNavModel();

        public Models.TreeNodeModel TreeRoot { get => SideNav.TreeRoot; }

        public MainWindowModel()
        {
        }
    }
}