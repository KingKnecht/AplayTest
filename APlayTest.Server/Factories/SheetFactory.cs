using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services.Infracstructure;
using sbardos.UndoFramework;

namespace APlayTest.Server.Factories
{

    public interface ISheetFactory : IObjectFactory<Sheet, SheetUndoable>
    {
        
    }

    public class SheetFactory : ISheetFactory
    {
        private readonly Dictionary<int, Sheet> _cache = new Dictionary<int, Sheet>();

        private readonly IUndoService _undoService;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IBlockSymbolFactory _blockSymbolFactory;


        public SheetFactory(IUndoService undoService, IConnectionFactory connectionFactory, IBlockSymbolFactory blockSymbolFactory)
        {
            _undoService = undoService;
            _connectionFactory = connectionFactory;
            _blockSymbolFactory = blockSymbolFactory;
        }

        public Sheet Create()
        {

            var id = IdGenerator.GetNextId();
            var sheet = new Sheet(id, _undoService,_blockSymbolFactory, _connectionFactory);

            _cache[id] = sheet;

            return sheet;
        }

        public Sheet Create(int id)
        {
            Sheet sheet;
            if (_cache.TryGetValue(id, out sheet))
            {
               return sheet;
            }

            sheet = new Sheet(id, _undoService, _blockSymbolFactory, _connectionFactory);
            _cache[id] = sheet;

            return sheet;
        }

        public Sheet Create(int id, ChangeSet changeSet)
        {
            throw new NotImplementedException();
        }

        public Sheet Create(SheetUndoable undoable, ChangeSet changeSet)
        {
            if (_cache.ContainsKey(undoable.Id))
                throw new InvalidOperationException("Id already exists in cache. This state is not correct. Id: " +
                                                    undoable.Id);

            var sheet = Create(undoable.Id);
            sheet.Name = undoable.Name;

            foreach (var connectionId in undoable.ConnectionIds)
            {
                sheet.Connections.Add(_connectionFactory.Create(connectionId, changeSet, sheet));
            }

            foreach (var blockId in undoable.BlockIds)
            {
                sheet.BlockSymbols.Add(_blockSymbolFactory.Create(blockId, changeSet,sheet));
            }

            return sheet;
        }

        public void Remove(int id)
        {
            _cache.Remove(id);
        }
    }
}
