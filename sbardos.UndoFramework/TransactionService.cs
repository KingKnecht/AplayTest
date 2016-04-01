using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace sbardos.UndoFramework
{
    public interface ITransactionService
    {
        Transaction StartTransaction(int clientId, string description);
        void EndTransaction(int clientId);
        void Add(Change change, int clientId, string description);

        void CancelTransaction(int clientId);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IUndoStackManager _undoStackManager;
        private readonly Dictionary<int, Transaction> _currentTransactions = new Dictionary<int, Transaction>();
        private static int _changeSetId;
        public TransactionService(IUndoStackManager undoStackManager)
        {
            _undoStackManager = undoStackManager;
        }

        public Transaction StartTransaction(int clientId, string description)
        {
            Transaction currentTransaction;
            if (_currentTransactions.TryGetValue(clientId, out currentTransaction))
            {
                if (currentTransaction.IsActive)
                {
                    return currentTransaction;
                }
            }

            Interlocked.Increment(ref _changeSetId);

            currentTransaction = new Transaction(clientId, new ChangeSet(clientId, _changeSetId, description));
            _currentTransactions[clientId] = currentTransaction;

            return currentTransaction;
        }

        public void EndTransaction(int clientId)
        {
            Transaction currentTransaction;
            if (_currentTransactions.TryGetValue(clientId, out currentTransaction))
            {
                if (currentTransaction.IsActive)
                {
                    currentTransaction.IsActive = false;
                    _undoStackManager.Push(currentTransaction.ChangeSet, clientId);
                }

            }
            else
            {
                throw new InvalidOperationException("No active transaction found for clientId: " + clientId);
            }
        }

        public void Add(Change change, int clientId, string description)
        {
            Transaction currentTransaction;
            if (_currentTransactions.TryGetValue(clientId, out currentTransaction))
            {
                if (currentTransaction.IsActive)
                {
                    currentTransaction.ChangeSet.Add(change);
                    return;
                }
            }

            var transaction = StartTransaction(clientId, description);
            transaction.ChangeSet.Add(change);
            EndTransaction(clientId);
        }

        public void CancelTransaction(int clientId)
        {
            Transaction currentTransaction;
            if (_currentTransactions.TryGetValue(clientId, out currentTransaction))
            {
                if (currentTransaction.IsActive)
                {
                    _undoStackManager.Cancel(currentTransaction.ChangeSet, clientId);
                    currentTransaction.IsActive = false;
                }
            }


        }
    }
}