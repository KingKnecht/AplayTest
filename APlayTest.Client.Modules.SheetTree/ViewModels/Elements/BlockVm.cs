using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace APlayTest.Client.Modules.SheetTree.ViewModels.Elements
{
    [Gemini.Modules.Toolbox.ToolboxItem(typeof(SheetDocumentViewModel),"Block","Items")]
    public class BlockVm : ElementViewModel
    {
        private readonly BlockSymbol _blockSymbol;
        private readonly Client _client;
        private double _x;
        private double _y;
        private double _inspectableX;
        private double _inspectableY;

        public BlockVm()
        {
            
        }

        public BlockVm(BlockSymbol blockSymbol, Client client)
        {
            _blockSymbol = blockSymbol;
            _client = client;

            _x = _blockSymbol.PositionX;
            _y = _blockSymbol.PositionY;

            _blockSymbol.PositionXChangeEventHandler += x => X = x;
            _blockSymbol.PositionYChangeEventHandler += y => Y = y;

            Id = blockSymbol.Id;
            

            AddInputConnector("Input1", Colors.BlueViolet);
            AddInputConnector("Input2", Colors.BlueViolet);
            OutputConnector = new OutputConnectorViewModel(this, "Output1", Colors.Black);
        }
        
        public int Id { get; private set; }

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
