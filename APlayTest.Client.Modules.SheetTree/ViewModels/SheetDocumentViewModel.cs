using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using APlayTest.Client.Modules.Inspector;
using APlayTest.Client.Modules.SheetTree.ViewModels.Elements;
using Caliburn.Micro;
using Gemini.Framework;


namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    [Export(typeof(SheetDocumentViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SheetDocumentViewModel : Document
    {
        private readonly Sheet _sheet;
        private string _name;
        public SheetDocumentViewModel(Sheet sheet, IInspectorTool inspectorTool)
        {
            _sheet = sheet;
            _name = _sheet.Name;
            _sheet.NameChangeEventHandler += _sheet_NameChangeEventHandler;
            SheetId = _sheet.Id;
            DisplayName = _name;

            _elements = new BindableCollection<ElementViewModel>();
            _connections = new BindableCollection<ConnectionViewModel>();

            _inspectorTool = inspectorTool;

            _sheet.BlockSymbolsAddEventHandler += _sheet_BlockSymbolsAddEventHandler;

            foreach (var blockSymbol in _sheet.BlockSymbols)
            {
                var block = AddBlock(blockSymbol);
            }


        }

     
        void _sheet_BlockSymbolsAddEventHandler(BlockSymbol newBlockSymbol)
        {
            AddBlock(newBlockSymbol);
        }

        private ElementViewModel AddBlock(BlockSymbol blockSymbol)
        {
            var block = new Block(blockSymbol);
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

        private readonly BindableCollection<ConnectionViewModel> _connections;
        private readonly IInspectorTool _inspectorTool;

        public IObservableCollection<ConnectionViewModel> Connections
        {
            get { return _connections; }
        }

        public IEnumerable<ElementViewModel> SelectedElements
        {
            get { return _elements.Where(x => x.IsSelected); }
        }

        //public TElement AddElement<TElement>(BlockSymbol item)
        //    where TElement : ElementViewModel, new()
        //{
        //    var element = new TElement { X = x, Y = y };
        //    _elements.Add(element);
        //    return element;
        //}

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
            Connections.RemoveRange(element.AttachedConnections);
            Elements.Remove(element);
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
                DisplayName = _name;
                _sheet.Name = _name;
                NotifyOfPropertyChange(() => Name);
            }
        }

         public int SheetId { get; set; }

    }
}