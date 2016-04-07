using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APlayTest.Client.Gemini.Properties;
using APlayTest.Client.Modules.Inspector;
using Gemini.Framework;
using Gemini.Framework.Menus;

namespace APlayTest.Client.Gemini.Startup
{

    public static class MenuDefinitions
    {

        [Export]
        public static ExcludeMenuItemGroupDefinition ExcludeFileNewOpenWindowMenuItemGroup = new ExcludeMenuItemGroupDefinition(global::Gemini.Modules.MainMenu.MenuDefinitions.FileNewOpenMenuGroup);

        [Export]
        public static ExcludeMenuItemGroupDefinition ExcludeFileSaveWindowMenuItemGroup = new ExcludeMenuItemGroupDefinition(global::Gemini.Modules.MainMenu.MenuDefinitions.FileSaveMenuGroup);

        [Export]
        public static ExcludeMenuItemGroupDefinition ExcludeCloseWindowMenuItemGroup = new ExcludeMenuItemGroupDefinition(global::Gemini.Modules.MainMenu.MenuDefinitions.FileCloseMenuGroup);

    }

    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {


        private readonly IInspectorTool _inspectorTool;

        public override IEnumerable<Type> DefaultTools
        {
            get { yield return typeof(IInspectorTool); }
        }

        [ImportingConstructor]
        public Module(IInspectorTool inspectorTool)
        {
            _inspectorTool = inspectorTool;
        }

        public override void Initialize()
        {
            Shell.ShowFloatingWindowsInTaskbar = true;
            Shell.ToolBars.Visible = true;


            Shell.StatusBar.AddItem("Not connected...", new GridLength(1, GridUnitType.Star));
            //Shell.StatusBar.AddItem("Ln 44", new GridLength(100));
            //Shell.StatusBar.AddItem("Col 79", new GridLength(100));


            Shell.ActiveDocumentChanged += (sender, e) => RefreshInspector();
            RefreshInspector();
        }

        private void RefreshInspector()
        {
            if (Shell.ActiveItem != null)
                _inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithObjectProperties(Shell.ActiveItem, pd => pd.ComponentType == Shell.ActiveItem.GetType())
                    .ToInspectableObject();
            else
                _inspectorTool.SelectedObject = null;
        }
    }
}
