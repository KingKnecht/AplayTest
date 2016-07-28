using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using APlayTest.Client.Modules.Inspector;

namespace APlayTest.Client.Modules.SheetTree.ViewModels.Elements
{
    [Gemini.Modules.Toolbox.ToolboxItem(typeof(SheetDocumentViewModel), "Connector", "Items")]
    public class ConnectorViewModel : SymbolBaseViewModel
    {
        private readonly Connector _connector;
        private readonly Client _client;
        private readonly IInspectorTool _inspectorTool;
        private double _x;
        private double _y;
        private string _path;

        public ConnectorViewModel()
        {
            Path = "M 0 4 H 10";
            
        }

        public ConnectorViewModel(Connector connector, Client client, IInspectorTool inspectorTool) :this()
        {
            _connector = connector;
            _client = client;
            _inspectorTool = inspectorTool;

            Id = _connector.Id;

            _x = _connector.PositionX;
            _y = _connector.PositionY;

            _connector.PositionXChangeEventHandler += x => X = x;
            _connector.PositionYChangeEventHandler += y => Y = y;
        }

        [Browsable(false)]
        public double X
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
        public double Y
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
                _connector.SetPosition(new AplayPoint(value, Y), _client);
            }
        }

        [DisplayName("Y")]
        public double InspectableY
        {
            get { return Y; }
            set
            {
                _connector.SetPosition(new AplayPoint(X, value), _client);
            }
        }

        public static double PreviewSizeX
        {
            get { return 20; }
        }

        public static double PreviewSizeY
        {
            get { return 8; }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if (value == _path) return;
                _path = value;
                NotifyOfPropertyChange(() => Path);
            }
        }

        public void SetPosition(double positionX, double positionY)
        {
            _connector.SetPosition(new AplayPoint(positionX, positionY), _client);
        }
    }
}