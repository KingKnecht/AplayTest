/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using sbardos.UndoFramework;

namespace APlayTest.Server
{
    public class Connector : APlayTest.Server.ConnectorSkeleton
    {
        public Sheet Sheet { get; set; }
        private readonly IUndoService _undoService;
        private readonly Action<Connector> _onPrepareRemoveCallBack;
        private readonly Factories.IConnectionFactory _connectionFactory;
        private readonly Action<int> _prepareRemoveCallback;

        /// <summary>
        /// Use this constructor to create instances in your code.
        /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
        ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
        /// </summary>

        public Connector()
        {
            /// TODO: add your code here
        }

        public Connector(int id, IUndoService undoService, Action<Connector> onPrepareRemoveCallBack, Sheet sheet, Factories.IConnectionFactory connectionFactory)
        {
            Sheet = sheet;
            Id = id;
            _undoService = undoService;
            _onPrepareRemoveCallBack = onPrepareRemoveCallBack;
            _connectionFactory = connectionFactory;

            _undoService.ActiveStateChanged += UndoServiceOnActiveStateChanged;
        }

        private void UndoServiceOnActiveStateChanged(object sender, ActiveStateChangedEventArgs e)
        {
            foreach (var change in e.ChangeSet.Where(c => c.OwnerId == Id))
            {
                if (change.ChangeReason == ChangeReason.Update)
                {
                    Connections.Clear();
                    var undoObject = (ConnectorUndoable)change.Undoable;
                    foreach (var connectionUndoable in undoObject.Connections)
                    {
                        Connections.Add(_connectionFactory.Create(connectionUndoable, e.ChangeSet));
                    }
                }


            }
        }

        public void Disconnect(Connection connection)
        {
            //Todo: Undo-Transaction?
            Connections.Remove(connection);
        }
        
        public override void onSetPosition(AplayPoint position__, Client client__)
        {
            if (Math.Abs(PositionX - position__.X) < double.Epsilon && Math.Abs(PositionY - position__.Y) < double.Epsilon)
            {
                return;
            }

            var oldState = new ConnectorUndoable(this);

            PositionX = position__.X;
            PositionY = position__.Y;

            _undoService.AddUpdate(oldState, new ConnectorUndoable(this), "Position of connector changed", client__.Id);
        }

        

        public void PrepareForRemove(int clientId)
        {
            _undoService.ActiveStateChanged -= UndoServiceOnActiveStateChanged;

            _onPrepareRemoveCallBack(this);
        }

        public ConnectorUndoable CreateUndoable()
        {
            return new ConnectorUndoable(this);
        }

        public string Dump()
        {
            var dump = "Connector: Id: " + Id;
           
            foreach (var connection in Connections)
            {
                dump += "\n\t" + connection.Dump();
            }

            return dump;
        }

      
    }

    public class ConnectorUndoable : IUndoable
    {
        public ConnectorUndoable(Connector connector)
        {
            Id = connector.Id;
            Position = new AplayPoint(connector.PositionX, connector.PositionY);
         
            Connections = connector.Connections.Select(c => new ConnectionUndoable(c)).ToList();

            SheetId = connector.Sheet.Id;
        }

        public IEnumerable<ConnectionUndoable> Connections { get; set; }

        public AplayPoint Position { get; set; }

        public int Id { get; private set; }
        public int SheetId { get; set; }

        public string Dump()
        {
            var str = "Connector Id: " + Id + ", Pos: " + Position + ", Conn.Ids: ";
            str += string.Join(", ", Connections.Select(c => c.Id));
            return str;
        }
    }
}
