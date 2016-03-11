using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using APlayTest.Client.Contracts;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{

    [Export(typeof(SheetTreeViewModel))]
    public class SheetTreeViewModel : Tool
    {

        [ImportingConstructor]
        public SheetTreeViewModel(IProjectAwareShell shell)
        {
            DisplayName = "Sheet Tree";

            shell.ProjectChanged += OnProjectChanged;

            //Sheets = new BindableCollection<Sheet>(new []{new Sheet(){Id = 1,Name = "Sheet 1"} });
        }

        void OnProjectChanged(object sender, SheetManager e)
        {

        }

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public IObservableCollection<Sheet> Sheets { get; set; }


    }
}
