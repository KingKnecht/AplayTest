using System;

namespace sbardos.UndoFramework
{
    /// <summary>
    /// Interface which acts as facade for the various classes of the undo framework.
    /// </summary>
    public interface IUndoService
    {
        Transaction StartTransaction(int clientId);
        void EndTransaction(int clientId);
        void Add(Change change, int clientId);
      
        event EventHandler<ActiveStateChangedEventArgs> ActiveStateChanged;
        event EventHandler<StackChangedEventArgs> StackChanged;
        bool CanUndo(int clientId);
        bool CanRedo(int clientId);
        void Undo(int clientId);
        void Redo(int clientId);
    }

    public class UndoService : IUndoService
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IUndoStackManager _undoStackManager;

        public UndoService(ITransactionManager transactionManager, IUndoStackManager undoStackManager)
        {
            _transactionManager = transactionManager;
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

        public Transaction StartTransaction(int clientId)
        {
            _undoStackManager.CreateStackForClient(clientId);
            return _transactionManager.StartTransaction(clientId);
        }

        public void EndTransaction(int clientId)
        {
            _transactionManager.EndTransaction(clientId);
        }

        public void Add(Change change, int clientId)
        {
            _transactionManager.Add(change, clientId);
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
        private readonly ITransactionManager _transactionManager;
        private readonly IUndoStackManager _undoStackManager;

        public UndoServiceFactory()
        {
            _undoStackManager = new UndoStackManager();
            _transactionManager = new TransactionManager(_undoStackManager);
        }
        public IUndoService Create()
        {
            return new UndoService(_transactionManager, _undoStackManager);
        }
    }
}