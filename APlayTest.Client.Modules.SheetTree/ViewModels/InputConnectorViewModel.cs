using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using APlayTest.Client.Modules.SheetTree.Factories;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    public class InputConnectorViewModel : ConnectorViewModel
    {
        private readonly Connector _connector;
        private readonly IConnectionViewModelFactory _connectionViewModelFactory;
        public event EventHandler SourceChanged;

        public InputConnectorViewModel(ElementViewModel element, Connector connector, IConnectionViewModelFactory connectionViewModelFactory)
            : base(element, "Input", Colors.Coral, connector)
        {
            _connector = connector;
            _connectionViewModelFactory = connectionViewModelFactory;

            if (connector.Connections.Any())
            {
                Connection = _connectionViewModelFactory.Create(connector.Connections.First());
                connector.Connections.First().To.ConnectionsRemoveEventHandler += ToOnConnectionsRemoveEventHandler;
            }


        }

        private void ToOnConnectionsRemoveEventHandler(Connection element)
        {
            Connector.Connections.First().To.ConnectionsRemoveEventHandler -= ToOnConnectionsRemoveEventHandler;
            //Todo: reagieren auf Zieltausch!!!

        }


        public override ConnectorDirection ConnectorDirection
        {
            get { return ConnectorDirection.Input; }
        }

        private ConnectionViewModel _connection;
        public ConnectionViewModel Connection
        {
            get { return _connection; }
            private set
            {
                _connection = value;

                if (_connection != null)
                {
                    if (_connection.To != this)
                    {
                        _connection.To = this;

                    }
                }

                RaiseSourceChanged();
                NotifyOfPropertyChange(() => Connection);
            }
        }

        private void OnSourceElementOutputChanged(object sender, EventArgs e)
        {
            RaiseSourceChanged();
        }

        public BitmapSource Value
        {
            get
            {
                if (Connection == null || Connection.From == null)
                    return null;

                return Connection.From.Value;
            }
        }



        private void RaiseSourceChanged()
        {
            var handler = SourceChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }


        public void AddConnection(ConnectionViewModel connection, Client client)
        {
            this.Connection = connection;
        }
    }
}