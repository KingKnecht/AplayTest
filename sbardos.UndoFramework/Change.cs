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
        IUndoable UndoObjectState { get; set; }
        IUndoable RedoObjectState { get; set; }
        int IndexAt { get; set; }
        string Dump();
    }

    public struct Change : IChange
    {
        private readonly IUndoable _newObjectState;
        
        /// <summary>
        /// Ctor for update.
        /// </summary>
        /// <param name="changeReason"></param>
        /// <param name="ownerId"></param>
        /// <param name="undoObjectState"></param>
        /// <param name="objectState"></param>
        public Change(ChangeReason changeReason, int ownerId, IUndoable undoObjectState, IUndoable objectState)
            : this()
        {

            UndoObjectState = undoObjectState;
            RedoObjectState = objectState;
            ChangeReason = changeReason;
            OwnerId = ownerId;
        }

        /// <summary>
        /// Ctor for InsertAt or Remove
        /// </summary>
        /// <param name="changeReason"></param>
        /// <param name="ownerId"></param>
        /// <param name="objectState"></param>
        /// <param name="indexAt">Index where the object should be inserted or removed from.</param>
        public Change(ChangeReason changeReason, int ownerId, IUndoable objectState, int indexAt)
            : this()
        {
            if (changeReason == ChangeReason.InsertAt)
            {
                UndoObjectState = new Optional();
                RedoObjectState = objectState;
            }
            else if (changeReason == ChangeReason.Remove)
            {
                UndoObjectState = objectState;
                RedoObjectState = new Optional(); 
            }
                ChangeReason = changeReason;
            OwnerId = ownerId;
            IndexAt = indexAt;
        }

        public ChangeReason ChangeReason { get; private set; }
        public int OwnerId { get; private set; }
        public int IndexAt { get; set; }
        public IUndoable UndoObjectState { get; set; }
        public IUndoable RedoObjectState { get; set; }

        public string Dump()
        {
            var dump = "ChangeReason: " + ChangeReason.ToString() + " , OwnerId: " + OwnerId + "\n";
            dump += "\tOldState: " + UndoObjectState.Dump() + "\n";
            dump += "\tNewState: " + RedoObjectState.Dump() + "\n";
            return dump;
        }
    }

    public class Optional : IUndoable
    {

        public int Id
        {
            get { return 0; }
        }

        public string Dump()
        {
            return "Empty";
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
        Remove,

        /// <summary>
        /// An item has been moved in a sorted collection
        /// </summary>
        Moved,

    }
}

