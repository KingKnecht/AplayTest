using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using APlayTest.Client.Contracts;
using APlayTest.Client.Modules.Inspector;
using APlayTest.Client.Modules.SheetTree.Commands;
using APlayTest.Client.Modules.SheetTree.Factories;
using APlayTest.Client.Modules.SheetTree.ViewModels.Elements;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using Reactive.Bindings;



namespace APlayTest.Client.Modules.SheetTree.ViewModels
{

    [Export(typeof(SheetDocumentViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SheetDocumentViewModel : Document, ICommandHandler<DeleteCommandDefinition>, ICommandHandler<DrawConnectLineCommandDefinition>
    {
        private readonly Sheet _sheet;
        private string _name;
        private readonly BindableCollection<ConnectionViewModel> _connections;
        private readonly IInspectorTool _inspectorTool;
        private readonly IAPlayAwareShell _shell;
        private readonly Action<SheetDocumentViewModel> _onOpenedChanged;
        private readonly Client _client;
        private bool _isOpen;
        private readonly BindableCollection<SymbolBaseViewModel> _symbolVms;
        private bool _isConnectionDrawMode;
        private SymbolBaseViewModel _ghost;

        [ImportingConstructor]
        public SheetDocumentViewModel(Sheet sheet,
            IInspectorTool inspectorTool,
            IAPlayAwareShell shell,
            Action<SheetDocumentViewModel> onOpenedChanged,
            Client client)
        {
            _sheet = sheet;
            _inspectorTool = inspectorTool;
            _shell = shell;

            _onOpenedChanged = onOpenedChanged;
            _client = client;

            _name = _sheet.Name;


            SheetId = _sheet.Id;
            DisplayName = _name;

            _symbolVms = new BindableCollection<SymbolBaseViewModel>();
            _connections = new BindableCollection<ConnectionViewModel>();

            foreach (var blockSymbol in _sheet.BlockSymbols)
            {
                _symbolVms.Add(new BlockViewModel(blockSymbol, _client, _inspectorTool));
            }

            foreach (var connector in _sheet.Connectors)
            {
                _symbolVms.Add(new ConnectorViewModel(connector, client, _inspectorTool));
            }

            foreach (var connection in _sheet.Connections)
            {
                _connections.Add(new ConnectionViewModel(connection, _client, _inspectorTool));
            }


            _sheet.NameChangeEventHandler += _sheet_NameChangeEventHandler;

            _sheet.BlockSymbolsAddEventHandler += OnBlockSymbolsAddEventHandler;
            _sheet.BlockSymbolsInsertAtEventHandler += OnBlockSymbolsInsertAtEventHandler;
            _sheet.BlockSymbolsRemoveAtEventHandler += OnBlockSymbolsRemoveAtEventHandler;

            _sheet.ConnectorsAddEventHandler += OnConnectorAddEventHandler;
            _sheet.ConnectorsInsertAtEventHandler += OnConnectorInsertAtEventHandler;
            _sheet.ConnectorsRemoveAtEventHandler += OnConnectorRemoveAtEventHandler;

            _sheet.ConnectionsAddEventHandler += OnConnectionsAddEventHandler;
            _sheet.ConnectionsRemoveEventHandler += OnConnectionsRemoveEventHandler;
            _sheet.ConnectionsRemoveAtEventHandler += OnConnectionsRemoveAtEventHandler;
            _sheet.ConnectionsInsertAtEventHandler += OnConnectionsInsertAtEventHandler;
        }

        private void OnConnectorAddEventHandler(Connector connector)
        {
            SymbolVms.Add(new ConnectorViewModel(connector, _client, _inspectorTool));
        }

        private void OnConnectorRemoveAtEventHandler(int pos, Connector connector)
        {
            var toBeRemoved = _symbolVms.FirstOrDefault(b => b.Id == connector.Id);
            if (toBeRemoved != null)
            {
                //Connectors.Remove(toBeRemoved);
                SymbolVms.Remove(toBeRemoved);
            }
        }

        private void OnConnectorInsertAtEventHandler(int pos, Connector connector)
        {
            var connectorVm = new ConnectorViewModel(connector, _client, _inspectorTool);

            _symbolVms.Insert(pos, connectorVm);
        }


        private void OnConnectionsInsertAtEventHandler(int indexAt, Connection connection)
        {
            if (_connections.All(c => c.Id != connection.Id))
            {
                _connections.Insert(indexAt, new ConnectionViewModel(connection, _client, _inspectorTool));
            }

            NotifyOfPropertyChange(() => ConnectionCount);
        }

        private void OnConnectionsRemoveAtEventHandler(int indexAt, Connection connection)
        {
            Connections.RemoveAt(indexAt);
        }

        private void OnConnectionsRemoveEventHandler(Connection connection)
        {
            var toBeRemoved = _connections.FirstOrDefault(b => b.Id == connection.Id);
            if (toBeRemoved != null)
            {
                Connections.Remove(toBeRemoved);
            }
        }

        private void OnConnectionsAddEventHandler(Connection connection)
        {
            Connections.Add(new ConnectionViewModel(connection, _client, _inspectorTool));

            NotifyOfPropertyChange(() => ConnectionCount);
        }

        private void OnBlockSymbolsAddEventHandler(BlockSymbol blockSymbol)
        {
            SymbolVms.Add(new BlockViewModel(blockSymbol, _client, _inspectorTool));
        }

        private void OnBlockSymbolsInsertAtEventHandler(int insertAt, BlockSymbol newBlockSymbol)
        {
            var block = new BlockViewModel(newBlockSymbol, _client, _inspectorTool);

            SymbolVms.Insert(insertAt, block);
        }

        private void OnBlockSymbolsRemoveAtEventHandler(int indexAt, BlockSymbol blockSymbol)
        {
            var toBeRemoved = _symbolVms.OfType<BlockViewModel>().FirstOrDefault(b => b.Id == blockSymbol.Id);
            if (toBeRemoved != null)
            {
                SymbolVms.Remove(toBeRemoved);
            }
        }

        public void DropElement(SymbolBaseViewModel symbolBaseViewModel)
        {
            if (symbolBaseViewModel is BlockViewModel)
            {
                var blockViewModel = (BlockViewModel) symbolBaseViewModel;
                var blockSymbol = _sheet.CreateBlockSymbol();
                blockSymbol.PositionX = blockViewModel.X;
                blockSymbol.PositionY = blockViewModel.Y;

                SymbolVms.Remove(_ghost);
                _ghost = null;

                _sheet.Add(blockSymbol, _client);

            }
            else if (symbolBaseViewModel is ConnectorViewModel)
            {
                var connectorViewModel = (ConnectorViewModel) symbolBaseViewModel;
                var connector = _sheet.CreateConnector();
                connector.PositionX = connectorViewModel.X;
                connector.PositionY = connectorViewModel.Y;

                SymbolVms.Remove(_ghost);
                _ghost = null;
                
                _sheet.AddConnector(connector, _client);
            }

        }
        
        void _sheet_NameChangeEventHandler(string NewName__)
        {
            Name = NewName__;
            DisplayName = NewName__;
        }

        public IObservableCollection<SymbolBaseViewModel> SymbolVms
        {
            get { return _symbolVms; }
        }


        public IObservableCollection<ConnectionViewModel> Connections
        {
            get { return _connections; }
        }

        //public IObservableCollection<ConnectorViewModel> Connectors
        //{
        //    get { return _connectors; }
        //}

        public IEnumerable<SymbolBaseViewModel> SelectedElements
        {
            get { return _symbolVms.Where(x => x.IsSelected); }
        }

        public override bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                _onOpenedChanged(this);
                NotifyOfPropertyChange(() => IsOpen);
            }
        }

        public override ICommand UndoCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand(p =>
                {
                    IsOpen = false;
                    TryClose(null);
                }, p => true));
            }
        }

        public ConnectionViewModel OnConnectionDragStarted(Point currentDragPoint)
        {


            _client.UndoManager.StartTransaction("New connection drag started");

            var connection = _sheet.CreateConnection();

            connection.FromPosition = new AplayPoint(currentDragPoint.X, currentDragPoint.Y);
            connection.ToPosition = new AplayPoint(currentDragPoint.X, currentDragPoint.Y);

            _sheet.AddConnection(connection, _client);

            var connectionViewModel = new ConnectionViewModel(connection, _client, _inspectorTool)
            {
                ToPosition = currentDragPoint
            };

            return connectionViewModel;
        }

        public void OnConnectionDragging(Point currentDragPoint, ConnectionViewModel connectionViewModel)
        {
            // If current drag point is close to an input connection, show its snapped position.
            //var nearbyConnector = FindNearbyInputConnector(currentDragPoint);
            //connectionViewModel.ToPosition = (nearbyConnector != null)
            //    ? nearbyConnector.Position
            //    : currentDragPoint;

            connectionViewModel.ToPosition = currentDragPoint;
        }

        public void OnConnectionDragCompleted(Point currentDragPoint, ConnectionViewModel newConnection)
        {
            //var nearbyConnector = FindNearbyInputConnector(currentDragPoint);

            //if (nearbyConnector == null)
            //{
            //    _client.UndoManager.CancelTransaction();
            //    return;
            //}

            //newConnection.AddToConnector(nearbyConnector, _client);

            newConnection.ToPosition = currentDragPoint;

            _client.UndoManager.EndTransaction();
        }



        private static bool AreClose(Point point1, Point point2, double distance)
        {
            return (point1 - point2).Length < distance;
        }

        public void DeleteElement(SymbolBaseViewModel block)
        {
            if (block != null)
            {
                var toBeDeleted = _sheet.BlockSymbols.FirstOrDefault(b => b.Id == block.Id);

                _sheet.Remove(toBeDeleted, _client);

            }

        }

        public void DeleteSelectedElements()
        {
            SymbolVms.Where(x => x.IsSelected)
                .ToList()
                .ForEach(DeleteElement);
            Connections.Where(x => x.IsSelected)
                .ToList()
                .ForEach(DeleteConnection);
        }

        private void DeleteConnection(ConnectionViewModel connection)
        {
            var toBeDeleted = _sheet.Connections.FirstOrDefault(c => c.Id == connection.Id);

            _sheet.RemoveConnection(toBeDeleted, _client);
        }

        public void OnSelectionChanged()
        {
            var selectedElements = SelectedElements.ToList();

            if (selectedElements.Count == 1)
                _inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithObjectProperties(selectedElements[0], x => true)
                    .ToInspectableObject();
            //else
            //    _inspectorTool.SelectedObject = null;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;

                _name = value;
                _sheet.SetName(_name, _client);
                NotifyOfPropertyChange(() => Name);
            }
        }

        public int SheetId { get; private set; }

        public void OnElementItemDragStarted(SymbolBaseViewModel symbolBaseViewModel, Point currentDragPoint)
        {


            if (symbolBaseViewModel is BlockViewModel)
            {
                var block = symbolBaseViewModel as BlockViewModel;
                _shell.UndoManager.StartTransaction("Moving Block: " + block.Name);

            }
            else if (symbolBaseViewModel is ConnectorViewModel)
            {
                var connectorVm = symbolBaseViewModel as ConnectorViewModel;
                _shell.UndoManager.StartTransaction("Moving Connector: " + connectorVm.Name);
            }
        }

        public void OnElementItemDragging(SymbolBaseViewModel symbolBaseViewModel, double horizontalChange, double verticalChange, double positionX, double positionY)
        {
            if (symbolBaseViewModel is BlockViewModel)
            {
                var block = symbolBaseViewModel as BlockViewModel;
                block.SetPosition(positionX, positionY);

            }
            else if (symbolBaseViewModel is ConnectorViewModel)
            {
                var connectorVm = symbolBaseViewModel as ConnectorViewModel;
                connectorVm.SetPosition(positionX, positionY);
            }


        }
        public void OnElementItemDragCompleted(SymbolBaseViewModel symbolBaseViewModel, Point currentDragPoint)
        {
            _shell.UndoManager.EndTransaction();

        }

        public void Update(Command command)
        {
            if (command.CommandDefinition is DeleteCommandDefinition)
            {
                command.Enabled = SelectedElements.Any() || Connections.Any(x => x.IsSelected);
            }
            else if (command.CommandDefinition is DrawConnectLineCommandDefinition)
            {
                command.Enabled = true;
            }

        }

        public Task Run(Command command)
        {
            if (command.CommandDefinition is DeleteCommandDefinition)
            {
                DeleteSelectedElements();
            }
            else if (command.CommandDefinition is DrawConnectLineCommandDefinition)
            {
                IsConnectionDrawMode = !IsConnectionDrawMode;
            }

            return TaskUtility.Completed;
        }

        public bool IsConnectionDrawMode
        {
            get { return _isConnectionDrawMode; }
            set
            {
                if (value == _isConnectionDrawMode) return;
                _isConnectionDrawMode = value;
                NotifyOfPropertyChange(() => IsConnectionDrawMode);
            }
        }

        public int ConnectionCount
        {
            get { return _connections.Count; }
        }


        public BlockViewModel CreateBlockViewModel()
        {
            return new BlockViewModel();
        }

        public void AddGhost(SymbolBaseViewModel element)
        {
            if (_ghost == null)
            {
                _ghost = element;
                SymbolVms.Add(_ghost);
            }

        }

        public SymbolBaseViewModel GetGhost()
        {
            return _ghost;
        }
    }
}