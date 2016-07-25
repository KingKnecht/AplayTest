namespace sbardos.UndoFramework
{
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

        public int Id { get; private set; }
        public string Description { get; private set; }

        public HistoryEntry(int changeSetId, string description)
        {
            Id = changeSetId;
            Description = description;
        }
    }
}