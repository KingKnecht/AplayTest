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
        IList<HistoryEntry> History { get; set; }
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
            
            History = new List<HistoryEntry>();

            _undoStack.CollectionChanged += sender =>
            {
                UndoStackCount = ((C5.LinkedList<ChangeSet>)sender).Count -1;
                CanUndo = UndoStackCount > 0;
            };

            _redoStack.CollectionChanged += sender =>
            {
                RedoStackCount = ((C5.LinkedList<ChangeSet>)sender).Count;
                CanRedo = RedoStackCount > 0;
            };

            //Empty ChangeSet = Initial state when nothing ever happend.
            var initialChangeSet = new ChangeSet(_clientId, 0, "Initial state")
            {
                new Change(ChangeReason.InsertAt,0, _clientId, new Empty(), 0)
            };

            Push(initialChangeSet);
        }

        public bool CanUndo { get; private set; }

        public bool CanRedo { get; private set; }


        public IList<HistoryEntry> History { get; set; }

        public void Push(ChangeSet changeSet)
        {
            if (changeSet.Count == 0)
                return; //do not push empty changes.

            var historyChanges = new List<HistoryChange>();

            if (RedoStackCount > 0)
            {
                historyChanges.AddRange(
                    _redoStack.Select(
                        changeSetOnRedoStack =>
                            new HistoryChange(new HistoryEntry(changeSetOnRedoStack.Id, changeSet.Description),
                                HistoryEntryChangeType.Removed)));

                _redoStack.Clear();
            }

            historyChanges.Add(new HistoryChange(new HistoryEntry(changeSet.Id, changeSet.Description), HistoryEntryChangeType.Added));

            UpdateHistory(historyChanges);

            _undoStack.Push(changeSet);

#if DEBUG
            DumpStack();
#endif

            OnStackChanged(new StackChangedEventArgs(historyChanges, _undoStack.Last.Id, _clientId));
        }

        private void UpdateHistory(List<HistoryChange> historyChanges)
        {
            foreach (var historyChange in historyChanges)
            {
                if (historyChange.ChangeType == HistoryEntryChangeType.Removed)
                {
                    History.Remove(historyChange.HistoryEntry);
                }
                else if (historyChange.ChangeType == HistoryEntryChangeType.Added)
                {
                    History.Add(historyChange.HistoryEntry);
                }
            }
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
                OnActiveChangeSetChanged(new ActiveStateChangedEventArgs(_redoStack.Last.AsReversed(), StateChangeDirection.Undo, _clientId));
                OnStackChanged(new StackChangedEventArgs(new List<HistoryChange>(), _undoStack.Last.Id, _clientId));
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
                OnStackChanged(new StackChangedEventArgs(new List<HistoryChange>(), _undoStack.Last.Id, _clientId));
            }
        }

        private void DumpStack()
        {

            //return;

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

        public void UndoRedoTo(HistoryEntry destinationEntry)
        {
            //Search the undo stack
            var foundUndoState = _undoStack.FirstOrDefault(cs => cs.Id == destinationEntry.Id);
            if (foundUndoState != null)
            {
                while (_undoStack.Last.Id != foundUndoState.Id)
                {
                    Undo();
                }
               
                return;
            }

            //Search the redo stack if needed.
            var foundRedoState = _redoStack.FirstOrDefault(cs => cs.Id == destinationEntry.Id);
            if (foundRedoState == null) 
                return;
            
            while (_undoStack.Last.Id != foundRedoState.Id)
            {
                Redo();
            }
        }
    }

    
}