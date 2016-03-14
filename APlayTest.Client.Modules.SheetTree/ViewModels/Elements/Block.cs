using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace APlayTest.Client.Modules.SheetTree.ViewModels.Elements
{
    public class Block : ElementViewModel
    {
        private readonly BlockSymbol _blockSymbol;
        private double _x;
        private double _y;

        public Block(BlockSymbol blockSymbol)
        {
            _blockSymbol = blockSymbol;

            _x= _blockSymbol.PositionX;
            _y = _blockSymbol.PositionY;

            _blockSymbol.PositionXChangeEventHandler += x => X = x;
            _blockSymbol.PositionYChangeEventHandler += y => Y = y;
            
            Id = blockSymbol.Id;
        }

        public int Id { get; private set; }

        public override double X
        {
            get { return _x; }
            set
            {
                if (value.Equals(_x)) return;
                _x = value;
                _blockSymbol.PositionX = _x;
                NotifyOfPropertyChange(() => X);
            }
        }

        public override double Y
        {
            get { return _y; }
            set
            {
                if (value.Equals(_y)) return;
                _y = value;
                _blockSymbol.PositionY = _y;
                NotifyOfPropertyChange(() => Y);
            }
        }
    }
}
