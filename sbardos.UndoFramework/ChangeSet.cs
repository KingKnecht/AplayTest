using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;

namespace sbardos.UndoFramework
{
    public class ChangeSet : IEnumerable<IChange>
    {
        public string Description { get;private set; }
        private readonly int _clientId;
        private readonly int _changeSetId;
        private readonly List<int> _orderOfChanges = new List<int>();
        private readonly Dictionary<int, IChange> _changes = new Dictionary<int, IChange>();


        public ChangeSet( int clientId,int changeSetId)
        {
            Description = "Description...";
            _clientId = clientId;
            _changeSetId = changeSetId;
        }

        public int Id
        {
            get { return _changeSetId; }
        }

        public void Add(IChange change)
        {

            if (!_changes.ContainsKey(change.OwnerId))
            {         
                _orderOfChanges.Add(change.OwnerId);
                _changes[change.OwnerId] = change;
            }
            else
            {
                switch (change.ChangeReason)
                {
                    case ChangeReason.InsertAt:
                        _changes.Add(change.OwnerId,change);
                        break;

                    case ChangeReason.Remove:

                        break;

                    case ChangeReason.Moved:

                        break;

                    case ChangeReason.Update:
                        _changes[change.OwnerId].RedoObjectState = change.RedoObjectState;
                        break;

                }
            }
        }

        public int Count
        {
            get { return _changes.Count; }
        }

        public int ClientId
        {
            get { return _clientId; }
        }


        public IEnumerator<IChange> GetEnumerator()
        {
            return _orderOfChanges.Select(id => _changes[id]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<IChange> Reverse()
        {
            return new Stack<IChange>(_orderOfChanges.Select(id => _changes[id]));
        }


        public IChange this[int i]
        {
            get { return _changes[_orderOfChanges[i]]; }
        }

        internal string Dump()
        {
            var dump = "\tChangeSet [" + Id + "]: ClientId: " + ClientId + ",  Count: " + Count;
            foreach (var change in _changes.Values)
            {
                dump += "\t" + change.Dump();
            }
            return dump;
        }
    }
}