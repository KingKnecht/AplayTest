using System;
using System.Collections.Generic;

namespace sbardos.UndoFramework
{
    public interface IUndoStackManager
    {
        void Push(ChangeSet changeSet, int clientId);
        void CreateStackForClient(int clientId);
        void DeleteStackForClient(int clientId);

        IUndoStack GetStackOfClient(int clientId);
        event EventHandler<ActiveStateChangedEventArgs> ActiveStateChanged;
        event EventHandler<StackChangedEventArgs> StackChanged;
        void Cancel(ChangeSet changeSet, int clientId);
        void UndoRedoTo(HistoryEntry destinationEntry, int clientId);
    }

    public class UndoStackManager : IUndoStackManager
    {
        private static readonly object _myLock = new object();
        private readonly Dictionary<int, UndoStack> _undoStacks = new Dictionary<int, UndoStack>();

        public UndoStackManager()
        {

        }

        public void Push(ChangeSet changeSet, int clientId)
        {
            lock (_myLock)
            {
                CreateStackForClient(clientId);
                _undoStacks[clientId].Push(changeSet);
            }
        }
        public void Cancel(ChangeSet changeSet, int clientId)
        {
            lock (_myLock)
            {
                OnActiveStateChanged(new ActiveStateChangedEventArgs(new ExternalChangeSet(changeSet.AsReversed(), StateChangeDirection.Undo),
                    clientId));
            }
        }

        public void UndoRedoTo(HistoryEntry destinationEntry, int clientId)
        {
            lock (_myLock)
            {
                _undoStacks[clientId].UndoRedoTo(destinationEntry);
            }
        }


        public void CreateStackForClient(int clientId)
        {
            lock (_myLock)
            {
                if (!_undoStacks.ContainsKey(clientId))
                {
                    var undoStack = new UndoStack(clientId);
                    undoStack.ActiveStateChanged += UndoStackOnActiveStateChanged;
                    undoStack.StackChanged += UndoStackOnStackChanged;
                    _undoStacks.Add(clientId, undoStack);
                }
            }
        }

        private void UndoStackOnStackChanged(object sender, StackChangedEventArgs e)
        {
            OnStackChanged(e);
        }

        private void UndoStackOnActiveStateChanged(object sender, ActiveStateChangedEventArgs e)
        {
            OnActiveStateChanged(e);
        }

        public void DeleteStackForClient(int clientId)
        {
            lock (_myLock)
            {
                _undoStacks[clientId].ActiveStateChanged -= UndoStackOnActiveStateChanged;
                _undoStacks[clientId].StackChanged -= StackChanged;

                _undoStacks.Remove(clientId);
            }
        }

        public event EventHandler<ActiveStateChangedEventArgs> ActiveStateChanged;
        public event EventHandler<StackChangedEventArgs> StackChanged;

        public IUndoStack GetStackOfClient(int clientId)
        {
            CreateStackForClient(clientId);
            return _undoStacks[clientId];
        }

        protected virtual void OnActiveStateChanged(ActiveStateChangedEventArgs e)
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
}