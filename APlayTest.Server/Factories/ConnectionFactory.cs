using System;
using System.Collections.Generic;
using System.Linq;
using APlayTest.Services.Infracstructure;
using sbardos.UndoFramework;

namespace APlayTest.Server.Factories
{

    public interface IConnectionFactory : ISheetHostedObjectFactory<Connection, ConnectionUndoable>
    {

    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly Dictionary<int, Connection> _cache = new Dictionary<int, Connection>();

        private readonly IUndoService _undoService;
        private IConnectorFactory _connectorFactory;

        public ConnectionFactory(IUndoService undoService)
        {
            _undoService = undoService;
        }

        public IConnectorFactory ConnectorFactory
        {
            get { return _connectorFactory; }
            set { _connectorFactory = value; }
        }

        public Factories.SheetFactory SheetFactory { get; set; }

        public Connection Create(Sheet sheet)
        {

            var id = IdGenerator.GetNextId();
            var connection = new Connection(id, _undoService, c => Remove(c.Id), sheet);

            _cache[id] = connection;

            return connection;
        }

        public Connection Create(int id, Sheet sheet)
        {
            Connection connection;
            if (_cache.TryGetValue(id, out connection))
            {
                return connection;
            }

            connection = new Connection(id, _undoService, c => Remove(c.Id), sheet);


            return connection;
        }

        public Connection Create(int id, ExternalChangeSet changeSet, Sheet sheet)
        {
            var connection = Create(id, sheet);

            //Todo: Shouldn't there be only one change? Check this..
            foreach (ExternalChange change in changeSet.Where(c => c.OwnerId == id))
            {
                if (change.ChangeReason == ChangeReason.Update)
                {
                    var undoable = (ConnectionUndoable)change.Undoable;
                    //connection.Color = undoable.Color Todo
                    connection.FromPosition = undoable.FromPosition;
                    connection.ToPosition = undoable.ToPosition;
                }

            }

            return connection;
        }

        public Connection Create(ConnectionUndoable undoable, ExternalChangeSet changeSet)
        {
            if (_cache.ContainsKey(undoable.Id))
                throw new InvalidOperationException("Id already exists in cache. This state is not correct. Id: " +
                                                    undoable.Id); //todo: still correct?

            if (ConnectorFactory == null)
                throw new InvalidOperationException("IConnectorFactory not injected.");

            var sheet = SheetFactory.Create(undoable.SheetId);
            var connection = Create(undoable.Id, sheet);

            connection.FromPosition = undoable.FromPosition;
            connection.ToPosition = undoable.ToPosition;

            connection.From = _connectorFactory.Create(undoable.FromId, sheet);
            connection.To = _connectorFactory.Create(undoable.ToId, sheet);

            return connection;
        }

        public void Remove(int id)
        {
            _cache.Remove(id);
        }
    }
}