using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using APlayTest.Client.Modules.SheetTree.Factories;
using APlayTest.Client.Modules.SheetTree.ViewModels.Elements;
using Caliburn.Micro;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    public class OutputConnectorViewModel : ConnectorViewModel
    {
        private readonly IConnectionViewModelFactory _connectionViewModelFactory;
        private readonly Func<BitmapSource> _valueCallback;

        public override ConnectorDirection ConnectorDirection
        {
            get { return ConnectorDirection.Output; }
        }

        private readonly BindableCollection<ConnectionViewModel> _connections;
        public IObservableCollection<ConnectionViewModel> Connections
        {
            get { return _connections; }
        }

        public BitmapSource Value
        {
            get { return null; } //_valueCallback(); }
        }
        
        public OutputConnectorViewModel(ElementViewModel element, Connector connector, IConnectionViewModelFactory connectionFactory)
            : base(element, "Output", Colors.Brown, connector)
        {
            _connectionViewModelFactory = connectionFactory;
            _connections = new BindableCollection<ConnectionViewModel>();
            
            foreach (var connection in connector.Connections)
            {
                var connectionVm = _connectionViewModelFactory.Create(connection);
                connectionVm.From = this;
                _connections.Add(connectionVm);
            }
        }


        public void Add(Connection connection)
        {
            Connector.Connections.Add(connection);
        }
        
    }
}