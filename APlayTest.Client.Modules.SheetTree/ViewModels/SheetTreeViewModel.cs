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
using Gemini.Modules.Inspector;
using Gemini.Modules.Inspector.Inspectors;
using Gemini.Modules.UndoRedo;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{

    [Export(typeof(SheetTreeViewModel))]
    public class SheetTreeViewModel : Tool
    {
        private readonly IAPlayAwareShell _shell;
        private readonly IInspectorTool _inspectorTool;
        private IObservableCollection<SheetDocumentViewModel> _sheets;
        private SheetDocumentViewModel _selectedSheet;


        [ImportingConstructor]
        public SheetTreeViewModel(IAPlayAwareShell shell, IInspectorTool inspectorTool)
        {
            _shell = shell;
            _inspectorTool = inspectorTool;
            
            DisplayName = "Sheet Tree";

            if (shell.Project != null)
            {
                if (shell.Project.SheetManager.Sheets != null)
                {
                    Sheets =
                        new BindableCollection<SheetDocumentViewModel>(shell.Project.SheetManager.Sheets.Select(s => new SheetDocumentViewModel(s, inspectorTool)));
                }
            }

            _shell.ProjectChanged += OnProjectChanged;

        }

        void OnProjectChanged(object sender, Project e)
        {
            Sheets =
                new BindableCollection<SheetDocumentViewModel>(
                    e.SheetManager.Sheets.Select(s => new SheetDocumentViewModel(s,_inspectorTool)));
        }
        

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public IObservableCollection<SheetDocumentViewModel> Sheets
        {
            get { return _sheets; }
            set
            {
                if (Equals(value, _sheets)) return;
                _sheets = value;
                NotifyOfPropertyChange(() => Sheets);
            }
        }

        public SheetDocumentViewModel SelectedSheet
        {
            get { return _selectedSheet; }
            set
            {
                if (Equals(value, _selectedSheet)) return;
                _selectedSheet = value;

                if (_selectedSheet != null)
                {
                    _shell.ActiveLayoutItem = _selectedSheet;

                    _inspectorTool.SelectedObject =
                        new InspectableObjectBuilder()
                     .WithEditor(_selectedSheet, x => x.Name, new TextBoxEditorViewModel<string>())
                     .WithEditor(_selectedSheet, s => s.SheetId, new TextBoxEditorViewModel<int>())
                      .ToInspectableObject();
                }
                else
                {
                    _inspectorTool.SelectedObject = null;
                }

                NotifyOfPropertyChange(() => SelectedSheet);
            }
        }


    }
}
