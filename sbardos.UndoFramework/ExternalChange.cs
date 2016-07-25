namespace sbardos.UndoFramework
{
    public class ExternalChange
    {
        public IUndoable Undoable { get; set; }
        public int OwnerId { get;internal set; }
        public ChangeReason ChangeReason { get;internal set; }
        public int IndexAt { get;internal set; }
        public int ItemId { get;internal set; }
    }
}