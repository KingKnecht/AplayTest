using System;
using System.ComponentModel.Composition;

namespace sbardos.UndoFramework
{
    /// <summary>
    /// Interface which acts as facade for the various classes of the undo framework.
    /// </summary>
    public interface IUndoService
    {
        Transaction StartTransaction(int clientId, string description);
        void EndTransaction(int clientId);
        //void Add(Change change, int clientId);
        void AddUpdate(IUndoable oldState, IUndoable newState,string description, int clientId);
        void AddInsert(int collectionOwnerId, IUndoable insertedObjectState, int indexAt, string description, int clientId);
        void AddRemove(int collectionOwnerId, IUndoable removedObjectState, int indexAt, string description, int clientId);
      
        event EventHandler<ActiveStateChangedEventArgs> ActiveStateChanged;
        event EventHandler<StackChangedEventArgs> StackChanged;
        bool CanUndo(int clientId);
        bool CanRedo(int clientId);
        void Undo(int clientId);
        void Redo(int clientId);
        void CancelTransaction(int clientId);
    }

    public class UndoService : IUndoService
    {
        private readonly ITransactionService _transactionService;
        private readonly IUndoStackManager _undoStackManager;

        public UndoService(ITransactionService transactionService, IUndoStackManager undoStackManager)
        {
            _transactionService = transactionService;
            _undoStackManager = undoStackManager;
            _undoStackManager.ActiveStateChanged += _undoStackManager_ActiveStateChanged;
            _undoStackManager.StackChanged += UndoStackManagerOnStackChanged;
        }

        private void UndoStackManagerOnStackChanged(object sender, StackChangedEventArgs e)
        {
            OnStackChanged(e);
        }

        void _undoStackManager_ActiveStateChanged(object sender, ActiveStateChangedEventArgs e)
        {
            OnActiveStateChanged(e);
        }

        public Transaction StartTransaction(int clientId, string description)
        {
            _undoStackManager.CreateStackForClient(clientId);
            return _transactionService.StartTransaction(clientId, description);
        }

        public void EndTransaction(int clientId)
        {
            _transactionService.EndTransaction(clientId);
        }

        public void AddUpdate(IUndoable oldState, IUndoable newState,string description, int clientId)
        {
            var updateChange = new Change(oldState.Id, oldState, newState);
            
            _transactionService.Add(updateChange,clientId,description);
        }

        public void AddInsert(int collectionOwnerId, IUndoable insertedObjectState, int indexAt,string description, int clientId)
        {
            var insertChange = new Change(ChangeReason.InsertAt, collectionOwnerId, insertedObjectState.Id,
                insertedObjectState, indexAt);

            _transactionService.Add(insertChange, clientId,description);
        }

        public void AddRemove(int collectionOwnerId, IUndoable removedObjectState, int indexAt,string description, int clientId)
        {
            var removeChange = new Change(ChangeReason.RemoveAt, collectionOwnerId, removedObjectState.Id,
                removedObjectState, indexAt);
            
            _transactionService.Add(removeChange, clientId,description);
        }
        
        public event EventHandler<ActiveStateChangedEventArgs> ActiveStateChanged;
        public event EventHandler<StackChangedEventArgs> StackChanged;

        public bool CanUndo(int clientId)
        {
            return _undoStackManager.GetStackOfClient(clientId).CanUndo;
        }

        public bool CanRedo(int clientId)
        {
            return _undoStackManager.GetStackOfClient(clientId).CanRedo;
        }


        public void Undo(int clientId)
        {
            _undoStackManager.GetStackOfClient(clientId).Undo();
        }

        public void Redo(int clientId)
        {
            _undoStackManager.GetStackOfClient(clientId).Redo();
        }

        public void CancelTransaction(int clientId)
        {
            _transactionService.CancelTransaction(clientId);
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

    public class UndoServiceFactory
    {
        private readonly ITransactionService _transactionService;
        private readonly IUndoStackManager _undoStackManager;

        public UndoServiceFactory()
        {
            _undoStackManager = new UndoStackManager();
            _transactionService = new TransactionService(_undoStackManager);
        }
        public IUndoService Create()
        {
            return new UndoService(_transactionService, _undoStackManager);
        }
    }
}