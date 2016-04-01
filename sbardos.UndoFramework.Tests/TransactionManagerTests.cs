using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sbardos.UndoFramework.Tests
{
    [TestClass]
    public class TransactionManagerTests
    {
        private IUndoStackManager _undoStackManager;
        private ITransactionService _transactionService;

        [TestInitialize]
        public void Init()
        {
            _undoStackManager = new UndoStackManager();
            _transactionService = new TransactionService(_undoStackManager);
                
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transaction_not_started_Test()
        {
            _transactionService.Add(
                new Change(ChangeReason.InsertAt, 1, new TestObject())
                , 100);
        }

        [TestMethod]
        public void StartTransaction_Not_EndTransaction()
        {
            var clientId = 101;

            _undoStackManager.CreateStackForClient(clientId);
            var undoStack = _undoStackManager.GetStackOfClient(clientId);
            _transactionService.StartTransaction(clientId);
            
            _transactionService.Add(
                new Change(ChangeReason.InsertAt, 1, new TestObject())
                , clientId);

            Assert.AreEqual(0, undoStack.UndoStackCount);

        }

        [TestMethod]
        public void StartTransaction_and_EndTransaction()
        {
            var clientId = 101;

            _undoStackManager.CreateStackForClient(clientId);
            var undoStack = _undoStackManager.GetStackOfClient(clientId);

            _transactionService.StartTransaction(clientId);

            _transactionService.Add(
                new Change(ChangeReason.InsertAt, 1, new TestObject())
                , clientId);

            _transactionService.EndTransaction(clientId);

            Assert.AreEqual(1, undoStack.UndoStackCount);

        }
        
    }
}