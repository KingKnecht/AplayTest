using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using APlayTest.Client.Modules.Inspector;
using APlayTest.Client.Modules.Inspector.Inspectors;
using APlayTest.Client.Modules.SheetTree.Factories;

namespace APlayTest.Client.Modules.SheetTree.ViewModels.Elements
{
    [Gemini.Modules.Toolbox.ToolboxItem(typeof(SheetDocumentViewModel),"Block","Items")]
    public class BlockVm : ElementViewModel
    {
        private readonly BlockSymbol _blockSymbol;
        private readonly Client _client;
        private readonly IConnectionViewModelFactory _connectionViewModelFactory;
        private readonly IInspectorTool _inspectorTool;
        private double _x;
        private double _y;
        private InputConnectorViewModel _selectedInputConnector;

        public BlockVm()
        {
            
        }

        public BlockVm(BlockSymbol blockSymbol, Client client, IConnectionViewModelFactory connectionViewModelFactory, IInspectorTool inspectorTool)
            :base(connectionViewModelFactory)
        {
            Id = blockSymbol.Id;

            _blockSymbol = blockSymbol;
            _client = client;
            _connectionViewModelFactory = connectionViewModelFactory;
            _inspectorTool = inspectorTool;

            _x = _blockSymbol.PositionX;
            _y = _blockSymbol.PositionY;

            _blockSymbol.PositionXChangeEventHandler += x => X = x;
            _blockSymbol.PositionYChangeEventHandler += y => Y = y;

        
            foreach (var inputConnector in blockSymbol.InputConnectors)
            {
                AddInputConnector(inputConnector);
            }

            if (blockSymbol.OutputConnector != null)
            {
                OutputConnector = new OutputConnectorViewModel(this, blockSymbol.OutputConnector, _connectionViewModelFactory);
            }
            
            
        }
        
        public int Id { get; private set; }

        public override InputConnectorViewModel SelectedInputConnector
        {
            get { return _selectedInputConnector; }
            set
            {
                if (Equals(value, _selectedInputConnector)) return;
                _selectedInputConnector = value;

                _inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithObjectProperties(_selectedInputConnector, x => true)
                    //.WithEditor(_selectedInputConnector, x => x.Position, new TextBoxEditorViewModel<string>())
                    //.WithEditor(_selectedInputConnector, viewModel => viewModel.Id, new TextBoxEditorViewModel<int>())
                    //.WithEditor(_selectedInputConnector, viewModel => viewModel.Position, new TextBoxEditorViewModel<Point>())
                    .ToInspectableObject();

                NotifyOfPropertyChange(() => SelectedInputConnector);
            }
        }

        [Browsable(false)]
        public override double X
        {
            get { return _x; }
            set
            {
                if (Math.Abs(_x - value) < Double.Epsilon)
                {
                    return;
                }
                _x = value;
                
                NotifyOfPropertyChange(() => X);
                NotifyOfPropertyChange(() => InspectableX);
            }
        }

        [Browsable(false)]
        public override double Y
        {
            get { return _y; }
            set
            {
                if (Math.Abs(_y - value) < Double.Epsilon)
                {
                    return;
                }
                _y = value;
                
                NotifyOfPropertyChange(() => Y);
                NotifyOfPropertyChange(() => InspectableY);
            }
        }

       [DisplayName("X")]
        public double InspectableX
        {
            get { return X; }
            set
            {
                _blockSymbol.SetPosition(new AplayPoint(value,Y),_client );
            }
        }

        [DisplayName("Y")]
        public double InspectableY
        {
            get { return Y; }
            set
            {
                _blockSymbol.SetPosition(new AplayPoint(X, value), _client);
            }
        }

       
        public void SetPosition(double positionX, double positionY)
        {
            _blockSymbol.SetPosition(new AplayPoint(positionX, positionY), _client);

        }
    }
}
