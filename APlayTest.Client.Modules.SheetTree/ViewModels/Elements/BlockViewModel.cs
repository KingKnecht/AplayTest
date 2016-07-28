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
using Caliburn.Micro;

namespace APlayTest.Client.Modules.SheetTree.ViewModels.Elements
{

    public abstract class SymbolBaseViewModel : PropertyChangedBase
    {
        private bool _isSelected;
        private string _name;
        public int Id { get; protected set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
    }


    [Gemini.Modules.Toolbox.ToolboxItem(typeof(SheetDocumentViewModel), "Block", "Items")]
    public class BlockViewModel : SymbolBaseViewModel
    {
        private readonly BlockSymbol _blockSymbol;
        private readonly Client _client;
        private readonly IInspectorTool _inspectorTool;
        private double _x;
        private double _y;

        public BlockViewModel()
        {

        }

        public BlockViewModel(BlockSymbol blockSymbol, Client client, IInspectorTool inspectorTool)
        {
            Id = blockSymbol.Id;

            _blockSymbol = blockSymbol;
            _client = client;
            _inspectorTool = inspectorTool;

            _x = _blockSymbol.PositionX;
            _y = _blockSymbol.PositionY;

            _blockSymbol.PositionXChangeEventHandler += x => X = x;
            _blockSymbol.PositionYChangeEventHandler += y => Y = y;

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
                _blockSymbol.SetPosition(new AplayPoint(value, Y), _client);
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





        public static double PreviewSize
        {
            get { return 100; }
        }

        public void SetPosition(double positionX, double positionY)
        {
            _blockSymbol.SetPosition(new AplayPoint(positionX, positionY), _client);
        }
    }
}
