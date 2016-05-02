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
using APlayTest.Client.Modules.SheetTree.ViewModels.Elements;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using Reactive.Bindings;
using sbardos.UndoFramework;


namespace APlayTest.Client.Modules.SheetTree.ViewModels
{

    [Export(typeof(SheetDocumentViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SheetDocumentViewModel : Document, ICommandHandler<DeleteCommandDefinition>
    {
        private readonly Sheet _sheet;
        private string _name;
        private readonly BindableCollection<ConnectionViewModel> _connections;
        private readonly IInspectorTool _inspectorTool;
        private readonly IAPlayAwareShell _shell;
        private readonly Action<SheetDocumentViewModel> _onOpenedChanged;
        private readonly Client _client;
        private bool _isOpen;

        public SheetDocumentViewModel(Sheet sheet, IInspectorTool inspectorTool, IAPlayAwareShell shell, Action<SheetDocumentViewModel> onOpenedChanged, Client client)
        {
            _sheet = sheet;
            _inspectorTool = inspectorTool;
            _shell = shell;

            _onOpenedChanged = onOpenedChanged;
            _client = client;

            _name = _sheet.Name;
            _sheet.NameChangeEventHandler += _sheet_NameChangeEventHandler;
            _sheet.BlockSymbolsAddEventHandler += OnBlockSymbolsAddEventHandler;
            SheetId = _sheet.Id;
            DisplayName = _name;

            _elements = new BindableCollection<ElementViewModel>();
            _connections = new BindableCollection<ConnectionViewModel>();



            foreach (var blockSymbol in _sheet.BlockSymbols)
            {
                var block = AddBlock(blockSymbol);
            }

            _sheet.NameChangeEventHandler += name => DisplayName = name;

            _sheet.BlockSymbolsInsertAtEventHandler += SheetOnBlockSymbolsInsertAtEventHandler;
            _sheet.BlockSymbolsRemoveAtEventHandler += SheetOnBlockSymbolsRemoveAtEventHandler;
        }

        private void OnBlockSymbolsAddEventHandler(BlockSymbol blockSymbol)
        {
            Elements.Add(new BlockVm(blockSymbol, _client));
        }

        private void SheetOnBlockSymbolsInsertAtEventHandler(int insertAt, BlockSymbol newBlockSymbol)
        {
            AddBlock(newBlockSymbol);
        }

        private void SheetOnBlockSymbolsRemoveAtEventHandler(int indexAt, BlockSymbol blockSymbol)
        {
            var toBeRemoved = _elements.OfType<BlockVm>().FirstOrDefault(b => b.Id == blockSymbol.Id);
            if (toBeRemoved != null)
            {
                Elements.Remove(toBeRemoved);
            }
        }

        public void DropElement(ElementViewModel blockVm)
        {
            var blockSymbol = _sheet.CreateBlockSymbol();
            blockSymbol.PositionX = blockVm.X;
            blockSymbol.PositionY = blockVm.Y;

            _sheet.Add(blockSymbol,_client);
        }


        private ElementViewModel AddBlock(BlockSymbol blockSymbol)
        {
            var block = new BlockVm(blockSymbol, _client);

            _elements.Add(block);

            return block;
        }

        void _sheet_NameChangeEventHandler(string NewName__)
        {
            Name = NewName__;
        }

        private readonly BindableCollection<ElementViewModel> _elements;
        public IObservableCollection<ElementViewModel> Elements
        {
            get { return _elements; }
        }


        public IObservableCollection<ConnectionViewModel> Connections
        {
            get { return _connections; }
        }

        public IEnumerable<ElementViewModel> SelectedElements
        {
            get { return _elements.Where(x => x.IsSelected); }
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

        public ConnectionViewModel OnConnectionDragStarted(ConnectorViewModel sourceConnector, Point currentDragPoint)
        {
            if (!(sourceConnector is OutputConnectorViewModel))
                return null;

            var connection = new ConnectionViewModel((OutputConnectorViewModel)sourceConnector)
            {
                ToPosition = currentDragPoint
            };

            Connections.Add(connection);

            return connection;
        }

        public void OnConnectionDragging(Point currentDragPoint, ConnectionViewModel connection)
        {
            // If current drag point is close to an input connector, show its snapped position.
            var nearbyConnector = FindNearbyInputConnector(currentDragPoint);
            connection.ToPosition = (nearbyConnector != null)
                ? nearbyConnector.Position
                : currentDragPoint;
        }

        public void OnConnectionDragCompleted(Point currentDragPoint, ConnectionViewModel newConnection, ConnectorViewModel sourceConnector)
        {
            var nearbyConnector = FindNearbyInputConnector(currentDragPoint);

            if (nearbyConnector == null || sourceConnector.Element == nearbyConnector.Element)
            {
                Connections.Remove(newConnection);
                return;
            }

            var existingConnection = nearbyConnector.Connection;
            if (existingConnection != null)
                Connections.Remove(existingConnection);

            newConnection.To = nearbyConnector;
        }

        private InputConnectorViewModel FindNearbyInputConnector(Point mousePosition)
        {
            return Elements.SelectMany(x => x.InputConnectors)
                .FirstOrDefault(x => AreClose(x.Position, mousePosition, 10));
        }

        private static bool AreClose(Point point1, Point point2, double distance)
        {
            return (point1 - point2).Length < distance;
        }

        public void DeleteElement(ElementViewModel element)
        {
            //Connections.RemoveRange(element.AttachedConnections);

            var block = element as BlockVm;
            if (block != null)
            {
                var toBeDeleted = _sheet.BlockSymbols.FirstOrDefault(b => b.Id == block.Id);

                _sheet.Remove(toBeDeleted, _client);

                return;
            }

        }

        public void DeleteSelectedElements()
        {
            Elements.Where(x => x.IsSelected)
                .ToList()
                .ForEach(DeleteElement);
        }

        public void OnSelectionChanged()
        {
            var selectedElements = SelectedElements.ToList();

            if (selectedElements.Count == 1)
                _inspectorTool.SelectedObject = new InspectableObjectBuilder()
                    .WithObjectProperties(selectedElements[0], x => true)
                    .ToInspectableObject();
            else
                _inspectorTool.SelectedObject = null;
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

        public void OnElementItemDragStarted(ElementViewModel itemViewModel, Point currentDragPoint)
        {
            var block = itemViewModel as BlockVm;
            if (block != null)
            {
                //Console.WriteLine("OnElementItemDragStarted()");
                _shell.UndoManager.StartTransaction("Moving Block: " + block.Name);
                return;
            }
        }

        public void OnElementItemDragging(ElementViewModel itemViewModel, double horizontalChange, double verticalChange, double positionX, double positionY)
        {
            var block = itemViewModel as BlockVm;
            if (block != null)
            {
                //Console.WriteLine("OnElementItemDragging()");
                block.SetPosition(positionX, positionY);
                return;
            }
        }
        public void OnElementItemDragCompleted(ElementViewModel itemViewModel, Point currentDragPoint)
        {
            var block = itemViewModel as BlockVm;
            if (block != null)
            {
                _shell.UndoManager.EndTransaction();
                return;
            }
        }

        public void Update(Command command)
        {
            command.Enabled = SelectedElements.Any();
        }

        public Task Run(Command command)
        {
            DeleteSelectedElements();
            return TaskUtility.Completed;
        }
    }
}