using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly IProjectAwareShell _shell;
        private readonly IInspectorTool _inspectorTool;
        private IObservableCollection<SheetVm> _sheets;
        private SheetVm _selectedSheet;


        [ImportingConstructor]
        public SheetTreeViewModel(IProjectAwareShell shell, IInspectorTool inspectorTool)
        {
            _shell = shell;
            _inspectorTool = inspectorTool;

            DisplayName = "Sheet Tree";

            _shell.ProjectChanged += OnProjectChanged;

        }

        void OnProjectChanged(object sender, Project e)
        {
            Sheets =
                new BindableCollection<SheetVm>(
                    e.SheetManager.Sheets.Select(s => new SheetVm(s)));


        }

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        public IObservableCollection<SheetVm> Sheets
        {
            get { return _sheets; }
            set
            {
                if (Equals(value, _sheets)) return;
                _sheets = value;
                NotifyOfPropertyChange(() => Sheets);
            }
        }

        public SheetVm SelectedSheet
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
                        .WithObjectProperties(_selectedSheet, x => true)
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

    public class SheetVm : Document
    {
        private readonly Sheet _sheet;
        private string _name;

        public SheetVm(Sheet sheet)
        {
            _sheet = sheet;
            _name = _sheet.Name;
            _sheet.NameChangeEventHandler += _sheet_NameChangeEventHandler;
        }
        
        void _sheet_NameChangeEventHandler(string NewName__)
        {
            Name = NewName__;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                _sheet.Name = _name;
                NotifyOfPropertyChange(() => Name);
            }
        }

        [Browsable(false)]
        public int Id { get; set; }

    }
}
