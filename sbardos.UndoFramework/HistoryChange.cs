namespace sbardos.UndoFramework
{

    public enum HistoryEntryChangeType
    {
        Added,
        Removed
    }

    public class HistoryChange
    {
        protected bool Equals(HistoryChange other)
        {
            return HistoryEntry.Equals(other.HistoryEntry);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HistoryChange)obj);
        }

        public override int GetHashCode()
        {
            return HistoryEntry.GetHashCode();
        }

        public HistoryEntry HistoryEntry { get; private set; }
        public HistoryEntryChangeType ChangeType { get; private set; }

        public HistoryChange(HistoryEntry historyEntry, HistoryEntryChangeType changeType)
        {
            HistoryEntry = historyEntry;
            ChangeType = changeType;
        }
    }
}