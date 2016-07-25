using System;
using System.Collections;
using System.Collections.Generic;

namespace sbardos.UndoFramework
{
    public class ExternalChangeSet : IEnumerable<ExternalChange>
    {

        private readonly List<ExternalChange> _changes = new List<ExternalChange>(); 

        public ExternalChangeSet(ChangeSet changeSet, StateChangeDirection changeDirection)
        {
            ClientId = changeSet.ClientId;

            if (changeDirection == StateChangeDirection.Undo)
            {
                foreach (IChange change in changeSet)
                {
                    var externalChange = new ExternalChange
                    {
                        OwnerId = change.OwnerId,
                        IndexAt = change.IndexAt,
                        ItemId = change.ItemId,
                        Undoable = change.UndoObjectState
                    };

                    switch (change.ChangeReason) //The initial change reason set when client adds an undoable change.
                    {

                        case ChangeReason.InsertAt:
                            externalChange.ChangeReason = ChangeReason.RemoveAt;
                            break;
                        case ChangeReason.Update:
                            externalChange.ChangeReason = ChangeReason.Update;
                            break;
                        case ChangeReason.RemoveAt:
                            externalChange.ChangeReason = ChangeReason.InsertAt;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    _changes.Add(externalChange);
                }
            }
            else if (changeDirection == StateChangeDirection.Redo)
            {
                foreach (IChange change in changeSet)
                {
                    var externalChange = new ExternalChange
                    {
                        OwnerId = change.OwnerId,
                        IndexAt = change.IndexAt,
                        ItemId = change.ItemId,
                        Undoable = change.RedoObjectState
                    };

                    switch (change.ChangeReason) //The initial change reason.
                    {
                        case ChangeReason.InsertAt:
                            externalChange.ChangeReason = ChangeReason.InsertAt;
                            break;
                        case ChangeReason.Update:
                            externalChange.ChangeReason = ChangeReason.Update;
                            break;
                        case ChangeReason.RemoveAt:
                            externalChange.ChangeReason = ChangeReason.RemoveAt;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    _changes.Add(externalChange);
                }
            }
        }

        public int ClientId { get; private set; }

        public IEnumerator<ExternalChange> GetEnumerator()
        {
            return _changes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return  _changes.GetEnumerator();
        }

    }
}