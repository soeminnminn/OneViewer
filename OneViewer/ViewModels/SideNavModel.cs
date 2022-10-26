using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace OneViewer.ViewModels
{
    internal class SideNavModel : Observable.ObservableObject
    {
        public Models.TreeNodeModel TreeRoot = Models.TreeNodeModel.Root;

        public SideNavModel()
        { }
    }
}