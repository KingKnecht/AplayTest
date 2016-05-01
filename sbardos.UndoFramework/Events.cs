using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undo.Client;

namespace sbardos.UndoFramework
{
    public enum StateChangeDirection
    {
        Redo,
        Undo
    }
    public class ActiveStateChangedEventArgs : EventArgs
    {
        public ChangeSet ChangeSet { get; private set; }
        public StateChangeDirection ChangeDirection { get; private set; }
        public int ClientId { get;private set; }

        public ActiveStateChangedEventArgs(ChangeSet changeSet, StateChangeDirection changeDirection, int clientId)
        {
            ChangeSet = changeSet;
            ChangeDirection = changeDirection;
            ClientId = clientId;
        }
    }

    public class StackChangedEventArgs : EventArgs
    {
        public IList<HistoryChanges> HistoryChanges { get; private set; }
        public int ActiveHistoryEntryId { get; private set; }
        public int ClientId { get;private set; }

        public StackChangedEventArgs(IList<HistoryChanges> historyChanges, int activeHistoryEntryId, int clientId)
        {
            HistoryChanges = historyChanges;
            ActiveHistoryEntryId = activeHistoryEntryId;
            ClientId = clientId;
        }
    }

    public class HistoryEntry
    {
        protected bool Equals(HistoryEntry other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HistoryEntry) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int Id { get;private set; }
        public string Description { get;private set; }

        public HistoryEntry(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
