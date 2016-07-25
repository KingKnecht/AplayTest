using System;
using System.Collections.Generic;
using APlayTest.Services.Infracstructure;
using DynamicData.Kernel;
using sbardos.UndoFramework;

namespace APlayTest.Server.Factories
{

    public interface IBlockSymbolFactory : ISheetHostedObjectFactory<BlockSymbol, BlockSymbolUndoable>
    {

    }

    public class BlockSymbolFactory : IBlockSymbolFactory
    {
        private readonly Dictionary<int, BlockSymbol> _cache = new Dictionary<int, BlockSymbol>();

        private readonly IUndoService _undoService;
        private readonly IConnectorFactory _connectorFactory;


        public BlockSymbolFactory(IUndoService undoService, IConnectorFactory connectorFactory)
        {
            _undoService = undoService;
            _connectorFactory = connectorFactory;
        }

        public ISheetFactory SheetFactory { get; set; }

        public BlockSymbol Create(Sheet sheet)
        {

            var id = IdGenerator.GetNextId();
            var blockSymbol = new BlockSymbol(id, _undoService, _connectorFactory, bs => Remove(bs.Id),sheet);

            _cache[id] = blockSymbol;

            return blockSymbol;
        }

        public BlockSymbol Create(int id, Sheet sheet)
        {
            BlockSymbol blockSymbol;
            if (_cache.TryGetValue(id, out blockSymbol))
            {
                return blockSymbol;
            }

            blockSymbol = new BlockSymbol(id, _undoService, _connectorFactory, bs => Remove(bs.Id), sheet);
            _cache[id] = blockSymbol;

            return blockSymbol;
        }

        public BlockSymbol Create(int id, ExternalChangeSet changeSet, Sheet sheet)
        {
            throw new NotImplementedException();
        }

        public BlockSymbol Create(BlockSymbolUndoable undoable, ExternalChangeSet changeSet)
        {
            if (_cache.ContainsKey(undoable.Id))
                throw new InvalidOperationException("Id already exists in cache. This state is not correct. Id: " +
                                                    undoable.Id);

            var blockSymbol = Create(undoable.Id,SheetFactory.Create(undoable.SheetId));
            blockSymbol.PositionX = undoable.Position.X;
            blockSymbol.PositionY = undoable.Position.Y;
            //Todo:blockSymbol.Size = undoable.Size;

            blockSymbol.InputConnectors.Clear();
            blockSymbol.OutputConnector = null;
            
            foreach (var connectorUndoable in undoable.InputConnectors)
            {
                blockSymbol.InputConnectors.Add(_connectorFactory.Create(connectorUndoable, changeSet));
            }

            //foreach (var inputConnectorId in undoable.InputConnectorIds)
            //{
            //    blockSymbol.InputConnectors.Add(_connectorFactory.Create(inputConnectorId, changeSet,
            //        SheetFactory.Create(undoable.SheetId)));
            //}

            if (undoable.OutputConnector != null)
            {
                blockSymbol.OutputConnector = _connectorFactory.Create(undoable.OutputConnector, changeSet);
            }

            //if (undoable.OutputConnectorId.HasValue)
            //{
            //    blockSymbol.OutputConnector = _connectorFactory.Create(undoable.OutputConnectorId.Value, changeSet,
            //        SheetFactory.Create(undoable.SheetId));
            //}

            return blockSymbol;
        }

        public void Remove(int id)
        {
            _cache.Remove(id);
        }
    }
}