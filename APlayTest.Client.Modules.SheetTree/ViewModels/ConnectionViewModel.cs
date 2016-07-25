using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APlay.Generated.Intern.Client;
using Caliburn.Micro;
using APlayTest.Client.Extensions;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    public class ConnectionViewModel : PropertyChangedBase
    {
        private OutputConnectorViewModel _from;
        private InputConnectorViewModel _to;
        private Point _fromPosition;
        private Point _toPosition;
        private readonly Connection _connection;
        private bool _isSelected;

        public ConnectionViewModel(Connection connection)
        {

            _connection = connection;
            Id = connection.Id;
            FromPosition = new Point(_connection.FromPosition.X, _connection.FromPosition.Y);
            ToPosition = new Point(_connection.ToPosition.X, _connection.ToPosition.Y);

            //Register for changes triggerd by the model.
            _connection.FromPositionChangeEventHandler += ConnectionOnFromPositionChangeEventHandler;
            _connection.ToPositionChangeEventHandler += ConnectionOnToPositionChangeEventHandler;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public OutputConnectorViewModel From
        {
            get { return _from; }
            set
            {
                if (_from != null)
                {
                    _from.Connections.Remove(this);
                }

                _from = value;

                if (_from != null)
                {
                   
                    _from.Add(_connection);
                }

                NotifyOfPropertyChange(() => From);
            }
        }

        public InputConnectorViewModel To
        {
            get { return _to; }
            set
            {
                if (_to != null)
                {
                    _to.Connection = null;
                }

                _to = value;

                if (_to != null)
                {
                    _connection.To = To.GetInternalConnector();
                    _connection.To.Connections.Add(_connection);
                    _to.Connection = this;
                }

                NotifyOfPropertyChange(() => To);
            }
        }




        public Point FromPosition
        {
            get { return _fromPosition; }
            set
            {
                if (_fromPosition != value)
                {
                    _fromPosition = value;
                    NotifyOfPropertyChange();
                }

                //Todo: Catch on server-side? Or both? To reduce traffic, do it here as well...
                var aplayPoint = new AplayPoint(_fromPosition.X, _fromPosition.Y);
                if (!_connection.FromPosition.SameAs(aplayPoint))
                {
                    _connection.FromPosition = aplayPoint;
                }
            }
        }


        public Point ToPosition
        {
            get { return _toPosition; }
            set
            {
                if (_toPosition != value)
                {
                    _toPosition = value;
                    //NotifyOfPropertyChange(() => ToPosition);
                    NotifyOfPropertyChange();
                    //Console.WriteLine("Connection ToPos: " + _toPosition);
                }


                //Todo: Catch on server-side? Or both? To reduce traffic, do it here as well...
                var aplayPoint = new AplayPoint(_toPosition.X, _toPosition.Y);
                if (!_connection.ToPosition.SameAs(aplayPoint))
                {
                    _connection.ToPosition = aplayPoint;

                }


            }
        }

        private void ConnectionOnFromPositionChangeEventHandler(AplayPoint newFromPosition)
        {
            FromPosition = new Point(newFromPosition.X, newFromPosition.Y);
            //Console.WriteLine("Connection FromPos: " + FromPosition);
        }

        private void ConnectionOnToPositionChangeEventHandler(AplayPoint newToPosition)
        {
            ToPosition = new Point(newToPosition.X, newToPosition.Y);
            //Console.WriteLine("Connection ToPos: " + ToPosition);
        }

        public int Id { get; private set; }

    }

    public enum ConnectorDataType
    {

    }
}
