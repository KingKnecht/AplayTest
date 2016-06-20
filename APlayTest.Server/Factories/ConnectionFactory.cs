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


        public ConnectionFactory(IUndoService undoService)
        {
            _undoService = undoService;
        }

        public IConnectorFactory ConnectorFactory { get; set; }
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

        public Connection Create(int id, ChangeSet changeSet, Sheet sheet)
        {
            var connection = Create(id, sheet);

            //Todo: Shouldn't there be only one change? Check this..
            foreach (IChange change in changeSet.Where(c => c.OwnerId == id && c.Handled == false))
            {
                change.Handled = true;
                if (change.ChangeReason == ChangeReason.Update)
                {
                    var undoable = (ConnectionUndoable)change.UndoObjectState;
                    //connection.Color = undoable.Color Todo
                    connection.FromPosition = undoable.FromPosition;
                    connection.ToPosition = undoable.ToPosition;

                    connection.From = ConnectorFactory.Create(undoable.From, changeSet);
                    //connection.From = ConnectorFactory.Create(undoable.FromId, changeSet, sheet);

                    //connection.To = ConnectorFactory.Create(undoable.ToId, changeSet, sheet);
                    connection.To = ConnectorFactory.Create(undoable.To, changeSet);
                }

            }

            return connection;
        }

        public Connection Create(ConnectionUndoable undoable, ChangeSet changeSet)
        {
            if (_cache.ContainsKey(undoable.Id))
                throw new InvalidOperationException("Id already exists in cache. This state is not correct. Id: " +
                                                    undoable.Id);

            if (ConnectorFactory == null)
                throw new InvalidOperationException("IConnectorFactory not injected.");

            var sheet = SheetFactory.Create(undoable.SheetId);
            var connection = Create(undoable.Id, sheet);

            connection.FromPosition = undoable.FromPosition;
            connection.ToPosition = undoable.ToPosition;

            if (undoable.From != null)
            {
                connection.From = ConnectorFactory.Create(undoable.From, changeSet);
                connection.From.Connections.Add(connection);
            }

            if (undoable.To != null)
            {
                connection.To = ConnectorFactory.Create(undoable.To, changeSet);
                connection.To.Connections.Add(connection);
            }

            //sheet.Connections.Add(connection);

            return connection;
        }

        public void Remove(int id)
        {
            _cache.Remove(id);
        }
    }
}