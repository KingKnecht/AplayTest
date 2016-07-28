using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APlay.Generated.Intern.Client;
using Caliburn.Micro;
using APlayTest.Client.Extensions;
using APlayTest.Client.Modules.Inspector;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    public class ConnectionViewModel : PropertyChangedBase
    {   private Point _fromPosition;
        private Point _toPosition;
        private readonly Connection _connection;
        private readonly Client _client;
        private bool _isSelected;

        public ConnectionViewModel(Connection connection, Client client,IInspectorTool inspectorTool)
        {

            _connection = connection;
            _client = client;
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
                    NotifyOfPropertyChange();
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
           // Console.WriteLine("Connection FromPos: " + FromPosition);
        }

        private void ConnectionOnToPositionChangeEventHandler(AplayPoint newToPosition)
        {
            ToPosition = new Point(newToPosition.X, newToPosition.Y);
            //Console.WriteLine("Connection ToPos: " + ToPosition);
        }

        public int Id { get; private set; }
        
    }

   }
