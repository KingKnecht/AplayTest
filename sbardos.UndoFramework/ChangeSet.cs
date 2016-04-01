using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;

namespace sbardos.UndoFramework
{
    public class ChangeSet : IEnumerable<IChange>
    {
        public string Description { get; private set; }
        private readonly int _clientId;
        private readonly int _changeSetId;
        private readonly List<IChange> _changes = new List<IChange>();

        internal ChangeSet(int clientId, int changeSetId, IEnumerable<IChange> changes, string description)
            : this(clientId, changeSetId, description)
        {
            _changes = new List<IChange>(changes);
        }

        public ChangeSet(int clientId, int changeSetId, string description)
        {
            Description = description;
            _clientId = clientId;
            _changeSetId = changeSetId;
        }

        public int Id
        {
            get { return _changeSetId; }
        }

        public void Add(IChange change)
        {
            var foundChange = _changes.FirstOrDefault(c => c.OwnerId == change.OwnerId);

            if (foundChange == null)
            {
                _changes.Add(change);
            }
            else
            {
                switch (change.ChangeReason)
                {
                    case ChangeReason.InsertAt:
                    case ChangeReason.RemoveAt:
                        _changes.Add(change);
                        break;
                    case ChangeReason.Update:
                        foundChange.RedoObjectState = change.RedoObjectState;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unknown change reason: " + change.ChangeReason);
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
            return _changes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<IChange> AsReverse()
        {
            return new Stack<IChange>(_changes);
        }
        public ChangeSet AsReversed()
        {
            return new ChangeSet(_clientId, Id, AsReverse(), Description);
        }

        public IChange this[int i]
        {
            get { return _changes[i]; }
        }

        internal string Dump()
        {
            var dump = "\tChangeSet [" + Id + "]: ClientId: " + ClientId + ",  Count: " + Count + "\n";

            foreach (var change in _changes)
            {
                dump += "\t" + change.Dump();
            }

            return dump;
        }

       
    }
}