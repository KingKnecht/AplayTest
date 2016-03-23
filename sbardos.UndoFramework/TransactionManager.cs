using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace sbardos.UndoFramework
{
    public interface ITransactionManager
    {
        Transaction StartTransaction(int clientId);
        void EndTransaction(int clientId);
        void Add(Change change, int clientId);

    }

    public class TransactionManager : ITransactionManager
    {
        private readonly IUndoStackManager _undoStackManager;
        private readonly Dictionary<int, Transaction> _currentTransactions = new Dictionary<int, Transaction>();
        private static int _changeSetId;
        public TransactionManager(IUndoStackManager undoStackManager)
        {
            _undoStackManager = undoStackManager;
        }

        public Transaction StartTransaction(int clientId)
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

            currentTransaction = new Transaction(clientId, new ChangeSet(clientId, _changeSetId));
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

        public void Add(Change change, int clientId)
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

            var transaction = StartTransaction(clientId);
            transaction.ChangeSet.Add(change);
            EndTransaction(clientId);
        }


    }
}