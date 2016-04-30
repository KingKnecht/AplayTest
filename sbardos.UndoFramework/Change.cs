using System;
using System.Collections;
using System.Dynamic;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace sbardos.UndoFramework
{
    public class Transaction
    {
        private readonly ChangeSet _changeSet;

        public Transaction(int ownerId, ChangeSet changeSet)
        {
            _changeSet = changeSet;
            OwnerId = ownerId;
            IsActive = true;
        }

        public int OwnerId { get; private set; }
        public int Id { get; set; }
        internal bool IsActive { get; set; }

        public ChangeSet ChangeSet
        {
            get { return _changeSet; }
        }
    }

    public interface IUndoable
    {
        int Id { get; }
        string Dump();

    }


    public interface IChange
    {
        ChangeReason ChangeReason { get; }
        int OwnerId { get; }
        int ItemId { get; }
        IUndoable UndoObjectState { get; set; }
        IUndoable RedoObjectState { get; set; }
        int IndexAt { get; set; }
        string Dump();
    }

    public class Change : IChange
    {
        private readonly IUndoable _newObjectState;

        /// <summary>
        /// Ctor for update.
        /// </summary>
        /// <param name="changeReason"></param>
        /// <param name="ownerId"></param>
        /// <param name="undoObjectState"></param>
        /// <param name="redoObjectState"></param>
        public Change(int ownerId, IUndoable undoObjectState, IUndoable redoObjectState)
        {
            UndoObjectState = undoObjectState;
            RedoObjectState = redoObjectState;
            ChangeReason = ChangeReason.Update;
            OwnerId = ownerId;
            ItemId = ownerId;
        }

        /// <summary>
        /// Ctor for InsertAt or RemoveAt
        /// </summary>
        /// <param name="changeReason"></param>
        /// <param name="ownerId"></param>
        /// <param name="objectState"></param>
        /// <param name="indexAt">Index where the object should be inserted or removed from.</param>
        public Change(ChangeReason changeReason, int ownerId,int itemId, IUndoable objectState, int indexAt)
           {
            if (changeReason == ChangeReason.InsertAt)
            {
                UndoObjectState = objectState;
                RedoObjectState = objectState;
            }
            else if (changeReason == ChangeReason.RemoveAt)
            {
                UndoObjectState = objectState;
                RedoObjectState = objectState;
            }
            else
            {
                throw new InvalidOperationException("Only " + ChangeReason.InsertAt + " and " + ChangeReason.RemoveAt +
                                                    " allowed for this constructor.");
            }
            ChangeReason = changeReason;
            OwnerId = ownerId;
            ItemId = itemId;
            IndexAt = indexAt;
        }

        public ChangeReason ChangeReason { get; private set; }
        public int OwnerId { get; private set; }
        public int ItemId { get;private set; }
        public int IndexAt { get; set; }
        public IUndoable UndoObjectState { get; set; }
        public IUndoable RedoObjectState { get; set; }

        public string Dump()
        {
            var dump = "Change:\n";
            dump += "\tChangeReason: " + ChangeReason.ToString() + " , OwnerId: " + OwnerId + "\n";
            dump += "\tOldState: " + UndoObjectState.Dump() + "\n";
            dump += "\tNewState: " + RedoObjectState.Dump() + "\n";
            return dump;
        }
    }

   

    public enum ChangeReason
    {
        /// <summary>
        ///  An item has been added
        /// </summary>
        InsertAt,

        /// <summary>
        ///  An item has been updated
        /// </summary>
        Update,

        /// <summary>
        ///  An item has removed
        /// </summary>
        RemoveAt,
    }
}

