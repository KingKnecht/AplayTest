using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services.Infracstructure;
using sbardos.UndoFramework;

namespace APlayTest.Server.Factories
{

    public interface IConnectorFactory : ISheetHostedObjectFactory<Connector, ConnectorUndoable>
    {

    }

    public class ConnectorFactory : IConnectorFactory
    {
        private readonly Dictionary<int, Connector> _cache = new Dictionary<int, Connector>();
        private readonly IUndoService _undoService;
        private readonly IConnectionFactory _connectionFactory;

        public ConnectorFactory(IUndoService undoService, IConnectionFactory connectionFactory)
        {
            _undoService = undoService;
            _connectionFactory = connectionFactory;
        }

        public ISheetFactory SheetFactory { get; set; }

        public Connector Create(Sheet sheet)
        {

            var id = IdGenerator.GetNextId();
            var connection = new Connector(id, _undoService, c => Remove(c.Id), sheet, _connectionFactory);

            _cache[id] = connection;

            return connection;
        }

        public Connector Create(int id, Sheet sheet)
        {
            Connector connector;
            if (_cache.TryGetValue(id, out connector))
            {
               return connector;
            }

            connector = new Connector(id, _undoService, c => Remove(c.Id),sheet, _connectionFactory);
            _cache[id] = connector;

            return connector;
        }

        public Connector Create(int id, ExternalChangeSet changeSet,Sheet sheet)
        {
            var connector = Create(id,sheet);

            //Todo: Shouldn't there onyl one change? Check this..
            foreach (ExternalChange change in changeSet.Where(c => c.OwnerId == id))
            {
              
                //Todo: Do we need change direction in this case? Direction should be part of the changeset in future.
                if (change.ChangeReason == ChangeReason.Update)
                {
                    var undoable = (ConnectorUndoable)change.Undoable;
                    connector.Direction = undoable.Direction;
                    connector.Position = undoable.Position;

                    foreach (var connection in undoable.Connections)
                    {
                        connector.Connections.Add(_connectionFactory.Create(connection, changeSet));
                    }

                    //foreach (var connectionId in undoable.ConnectionIds)
                    //{
                    //    connector.Connections.Add(_connectionFactory.Create(connectionId, changeSet, sheet));
                    //}
                }
            }


            return connector;
        }

        public Connector Create(ConnectorUndoable undoable, ExternalChangeSet changeSet)
        {
            if (_cache.ContainsKey(undoable.Id))
            {
                //throw new InvalidOperationException("Id already exists in cache. This state is not correct. Id: " +
                //                                    undoable.Id);
                var x = 10;
            }
            var connector = Create(undoable.Id, SheetFactory.Create(undoable.SheetId));
            connector.Direction = undoable.Direction;
            connector.Position = undoable.Position;
            
            return connector;
        }

        public void Remove(int id)
        {
            _cache.Remove(id);
        }
    }
}
