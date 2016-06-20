using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using APlayTest.Client.Modules.SheetTree.Factories;
using Caliburn.Micro;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    public abstract class ConnectorViewModel : PropertyChangedBase
    {
        public event EventHandler PositionChanged;

        private readonly ElementViewModel _element;
        public ElementViewModel Element
        {
            get { return _element; }
        }

        public int Id { get; protected set; }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly Color _color = Colors.Black;
        protected readonly Connector Connector;
        private readonly IConnectionViewModelFactory _connectionViewModelFactory;

        public Color Color
        {
            get { return _color; }
        }

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set
            {
                //Needed? See below.
                if (_position != value)
                {
                    _position = value;
                    NotifyOfPropertyChange(() => Position);
                }


                //Needed? Both? Quick debugging tells that both cases appear...
                var aplayPoint = new AplayPoint(_position.X, _position.Y);
                if (Connector.Position != aplayPoint)
                {
                    Connector.Position = aplayPoint;
                }

            }
        }

        public abstract ConnectorDirection ConnectorDirection { get; }


        protected ConnectorViewModel(ElementViewModel element, string name, Color color, Connector connector)
        {
            _element = element;
            _name = name;
            _color = color;
            Connector = connector;

            Id = Connector.Id;
            Position = new Point(Connector.Position.X, Connector.Position.Y);

            Connector.PositionChangeEventHandler +=
                position => { Position = new Point(Connector.Position.X, Connector.Position.Y); };
        }

        public Connector GetInternalConnector()
        {
            return Connector;
        }

    }
}