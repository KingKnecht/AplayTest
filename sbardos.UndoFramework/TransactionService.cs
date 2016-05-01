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
        private static readonly object _myLock = new object();
        private readonly IUndoStackManager _undoStackManager;
        private readonly Dictionary<int, Transaction> _currentTransactions = new Dictionary<int, Transaction>();
        private static int _changeSetId;
        public TransactionService(IUndoStackManager undoStackManager)
        {
            _undoStackManager = undoStackManager;
        }

        public Transaction StartTransaction(int clientId, string description)
        {
            lock (_myLock)
            {
                Transaction currentTransaction;
                if (_currentTransactions.TryGetValue(clientId, out currentTransaction))
                {
                    if (currentTransaction.IsActive)
                    {
                        currentTransaction.IncrementRefCounter();
                        //Console.WriteLine("Re-used transaction: " + currentTransaction.ChangeSet.Description);
                        return currentTransaction;
                    }
                }

                Interlocked.Increment(ref _changeSetId);

                currentTransaction = new Transaction(clientId, new ChangeSet(clientId, _changeSetId, description));
                _currentTransactions[clientId] = currentTransaction;

                //Console.WriteLine("New transaction created: " + description);

                return currentTransaction;
            }
        }

        public void Add(Change change, int clientId, string description)
        {
            var transaction = StartTransaction(clientId, description);
            transaction.ChangeSet.Add(change);

            EndTransaction(clientId);

        }

        public void EndTransaction(int clientId)
        {
            Transaction currentTransaction;
            if (_currentTransactions.TryGetValue(clientId, out currentTransaction))
            {
                if (currentTransaction.IsActive)
                {
                    currentTransaction.DecrementRefCounter();
                    if (!currentTransaction.IsActive)
                    {
                        _undoStackManager.Push(currentTransaction.ChangeSet, clientId);    
                    }
                    
                }

            }
            else
            {
                throw new InvalidOperationException("No active transaction found for clientId: " + clientId);
            }
        }

        

        public void CancelTransaction(int clientId)
        {
            Transaction currentTransaction;
            if (_currentTransactions.TryGetValue(clientId, out currentTransaction))
            {
                if (currentTransaction.IsActive)
                {
                    _undoStackManager.Cancel(currentTransaction.ChangeSet, clientId);
                    currentTransaction.Cancel();
                }
            }


        }
    }
}