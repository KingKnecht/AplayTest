using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using APlayTest.Client.Contracts;
using APlayTest.Client.Modules.Inspector;
using APlayTest.Client.Modules.Inspector.Inspectors;
using APlayTest.Client.Modules.SheetTree.Factories;
using Caliburn.Micro;
using DynamicData;
using Gemini.Framework;
using Gemini.Framework.Services;

using Gemini.Modules.UndoRedo;
using Reactive.Bindings.Extensions;
using sbardos.UndoFramework;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{

    [Export(typeof(SheetTreeViewModel))]
    public class SheetTreeViewModel : Tool
    {
        private readonly IAPlayAwareShell _shell;
        private readonly IInspectorTool _inspectorTool;
        private readonly IConnectionViewModelFactory _connectionViewModelFactory;
        private IObservableCollection<SheetDocumentViewModel> _sheets;
        private SheetDocumentViewModel _selectedSheet;


        [ImportingConstructor]
        public SheetTreeViewModel(IAPlayAwareShell shell, IInspectorTool inspectorTool, IConnectionViewModelFactory connectionViewModelFactory)
        {
            _shell = shell;
            _inspectorTool = inspectorTool;
            _connectionViewModelFactory = connectionViewModelFactory;


            DisplayName = "Sheet Tree";
            Sheets = new BindableCollection<SheetDocumentViewModel>();

            _shell.ProjectChanged += OnProjectChanged;

            if (shell.Project != null)
            {
                if (shell.Project.SheetManager.Sheets != null)
                {
                    Sheets.AddRange(shell.Project.SheetManager.Sheets.Select(sheet => new SheetDocumentViewModel(sheet, inspectorTool, shell, OnOpenedChanged, _shell.Client, _connectionViewModelFactory)));
                }
            }


        }

        public void OnOpenedChanged(IDocument vm)
        {
            if (vm.IsOpen)
            {
                _shell.ActiveLayoutItem = vm;
            }
            else
            {
                _shell.CloseDocument(vm);
            }

        }

        void OnProjectChanged(object sender, Project e)
        {
            Sheets.AddRange(e.SheetManager.Sheets.Select(s => new SheetDocumentViewModel(s, _inspectorTool, _shell, OnOpenedChanged, _shell.Client, _connectionViewModelFactory)));
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
                    _inspectorTool.SelectedObject =
                          new InspectableObjectBuilder()
                       .WithEditor(_selectedSheet, x => x.Name, new TextBoxEditorViewModel<string>())
                       .WithEditor(_selectedSheet, s => s.SheetId, new TextBoxEditorViewModel<int>())
                       .WithEditor(_selectedSheet, s => s.ConnectionCount, new TextBoxEditorViewModel<int>())
                        .ToInspectableObject();

                    if (_selectedSheet.IsOpen && _selectedSheet.IsActive == false)
                    {
                        _shell.ActiveLayoutItem = _selectedSheet;
                    }
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
