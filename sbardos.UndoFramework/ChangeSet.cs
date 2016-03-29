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

        public ChangeSet(int clientId, int changeSetId, IEnumerable<IChange> changes)
            : this(clientId, changeSetId)
        {
            _changes = new List<IChange>(changes);
        }

        public ChangeSet(int clientId, int changeSetId)
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
            var foundChange = _changes.FirstOrDefault(c => c.OwnerId == change.OwnerId);

            if (foundChange == null)
            {
                switch (change.ChangeReason)
                {
                    case ChangeReason.InsertAt:
                        _changes.Add(change);
                        break;
                    case ChangeReason.Update:
                        //Check if the updated object is part of a list change.
                        var insertionChange =
                            _changes.FirstOrDefault(
                                c => c.ChangeReason == ChangeReason.InsertAt && c.RedoObjectState.Id == change.OwnerId);

                        if (insertionChange != null)
                        {
                            //If yes, update the list insert-change instead. So the inserted object will have the correct state when undo/redo.
                            insertionChange.RedoObjectState = change.RedoObjectState;
                            insertionChange.UndoObjectState = change.UndoObjectState;
                        }
                        else
                        {
                            _changes.Add(change);
                        }

                        break;
                    case ChangeReason.RemoveAt:
                        //Check if there was already an update-change of the to be removed object.
                        var updateChange =
                            _changes.FirstOrDefault(
                                c => c.ChangeReason == ChangeReason.Update && c.UndoObjectState.Id == change.UndoObjectState.Id);

                        if (updateChange != null)
                        {
                            //If yes, ignore the updated state. Instead use the undo state as state of the undo/redo objects.
                            //Then remove the update-change and add it to the remove-change instead.
                            //So the object will have the correct state when undo/redo.
                            change.UndoObjectState = updateChange.UndoObjectState;
                            change.RedoObjectState = updateChange.UndoObjectState;
                            _changes.Remove(updateChange);    
                        }
                        
                        _changes.Add(change);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unknown change reason: " + change.ChangeReason);
                }

            }
            else
            {
                switch (change.ChangeReason)
                {
                    case ChangeReason.InsertAt:
                        _changes.Add(change);
                        break;

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

        private IEnumerable<IChange> Reverse()
        {
            return new Stack<IChange>(_changes);
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

        public ChangeSet ToReversed()
        {
            return new ChangeSet(_clientId, Id, Reverse());
        }
    }
}