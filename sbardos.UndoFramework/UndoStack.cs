using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using sbardos.UndoFramework.Annotations;

namespace sbardos.UndoFramework
{



    public interface IUndoStack
    {
        event EventHandler<ActiveStateChangedEventArgs> ActiveStateChanged;
        event EventHandler<StackChangedEventArgs> StackChanged;
        bool CanUndo { get; }
        bool CanRedo { get; }
        int UndoStackCount { get; set; }
        int RedoStackCount { get; set; }
        void Push(ChangeSet changeSet);
        void Undo();
        void Redo();
        ChangeSet GetActiveChangeSet();
    }

    public class UndoStack : IUndoStack
    {
        private readonly int _clientId;
        private readonly int _maxStackSize;
        private readonly C5.LinkedList<ChangeSet> _undoStack = new C5.LinkedList<ChangeSet>();
        private readonly C5.LinkedList<ChangeSet> _redoStack = new C5.LinkedList<ChangeSet>();
        public event EventHandler<ActiveStateChangedEventArgs> ActiveStateChanged;
        public event EventHandler<StackChangedEventArgs> StackChanged;


        public UndoStack(int clientId)
        {
            _clientId = clientId;

            _undoStack.CollectionChanged += sender =>
            {
                UndoStackCount = ((C5.LinkedList<ChangeSet>)sender).Count - 1;
                CanUndo = UndoStackCount > 0;
            };

            _redoStack.CollectionChanged += sender =>
            {
                RedoStackCount = ((C5.LinkedList<ChangeSet>)sender).Count;
                CanRedo = RedoStackCount > 0;
            };

            //Empty ChangeSet = Initial state when nothing ever happend.
            var initialChangeSet = new ChangeSet(_clientId, 0)
            {
                new Change(ChangeReason.InsertAt, _clientId, new Empty(), 0)
            };

            _undoStack.Add(initialChangeSet);
        }

        public bool CanUndo { get; private set; }

        public bool CanRedo { get; private set; }


        public void Push(ChangeSet changeSet)
        {
            var historyChanges = new List<HistoryChanges>();
            if (changeSet.Count == 0)
                return; //do not push empty changes.

            if (RedoStackCount > 0)
            {
                historyChanges.AddRange(
                    _redoStack.Select(
                        changeSetOnRedoStack =>
                            new HistoryChanges(new HistoryEntry(changeSetOnRedoStack.Id, "Description..."),
                                HistoryEntryChangeType.Removed)));

                _redoStack.Clear();
            }

            historyChanges.Add(new HistoryChanges(new HistoryEntry(changeSet.Id, "Description..."), HistoryEntryChangeType.Added));

            _undoStack.Push(changeSet);

#if DEBUG
            DumpStack();
#endif
                       
            OnStackChanged(new StackChangedEventArgs(historyChanges, _undoStack.Last.Id, _clientId));
        }

        public void Undo()
        {
            if (CanUndo)
            {
                var current = _undoStack.Pop();
                _redoStack.Push(current);
#if DEBUG
                DumpStack();
#endif
                OnActiveChangeSetChanged(new ActiveStateChangedEventArgs(_redoStack.Last, StateChangeDirection.Undo, _clientId));
                OnStackChanged(new StackChangedEventArgs(new List<HistoryChanges>(), _undoStack.Last.Id, _clientId));
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                var current = _redoStack.Pop();
                _undoStack.Push(current);

#if DEBUG
                DumpStack();
#endif

                OnActiveChangeSetChanged(new ActiveStateChangedEventArgs(_undoStack.Last, StateChangeDirection.Redo, _clientId));
                OnStackChanged(new StackChangedEventArgs(new List<HistoryChanges>(), _undoStack.Last.Id,_clientId));
            }
        }

        private void DumpStack()
        {
            Console.WriteLine("Undo Stack #: " + UndoStackCount);
            foreach (var changeSet in _undoStack)
            {
                Console.WriteLine(changeSet.Dump());
            }

            Console.WriteLine("Redo Stack #: " + RedoStackCount);
            foreach (var changeSet in _redoStack)
            {
                Console.WriteLine(changeSet.Dump());
            }
        }

        [NotNull]
        public ChangeSet GetActiveChangeSet()
        {
            return _undoStack.Last;
        }

        public int UndoStackCount { get; set; }
        public int RedoStackCount { get; set; }

        private struct Empty : IUndoable
        {
            public int Id
            {
                get { return 0; }
            }

            public string Dump()
            {
                return "Initial, empty change";
            }
        }

        protected virtual void OnActiveChangeSetChanged(ActiveStateChangedEventArgs e)
        {
            var handler = ActiveStateChanged;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnStackChanged(StackChangedEventArgs e)
        {
            var handler = StackChanged;
            if (handler != null) handler(this, e);
        }
    }

    public enum HistoryEntryChangeType
    {
        Added,
        Removed
    }

    public class HistoryChanges
    {
        protected bool Equals(HistoryChanges other)
        {
            return HistoryEntry.Equals(other.HistoryEntry);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HistoryChanges) obj);
        }

        public override int GetHashCode()
        {
            return HistoryEntry.GetHashCode();
        }

        public HistoryEntry HistoryEntry { get;private set; }
        public HistoryEntryChangeType ChangeType { get; private set; }

        public HistoryChanges(HistoryEntry historyEntry, HistoryEntryChangeType changeType)
        {
            HistoryEntry = historyEntry;
            ChangeType = changeType;
        }
    }
}