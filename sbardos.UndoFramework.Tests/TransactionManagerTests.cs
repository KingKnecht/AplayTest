using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sbardos.UndoFramework.Tests
{
    [TestClass]
    public class TransactionManagerTests
    {
        private IUndoStackManager _undoStackManager;
        private ITransactionManager _transactionManager;

        [TestInitialize]
        public void Init()
        {
            _undoStackManager = new UndoStackManager();
            _transactionManager = new TransactionManager(_undoStackManager);
                
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transaction_not_started_Test()
        {
            _transactionManager.Add(
                new Change(ChangeReason.InsertAt, 1, new TestObject())
                , 100);
        }

        [TestMethod]
        public void StartTransaction_Not_EndTransaction()
        {
            var clientId = 101;

            _undoStackManager.CreateStackForClient(clientId);
            var undoStack = _undoStackManager.GetStackOfClient(clientId);
            _transactionManager.StartTransaction(clientId);
            
            _transactionManager.Add(
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

            _transactionManager.StartTransaction(clientId);

            _transactionManager.Add(
                new Change(ChangeReason.InsertAt, 1, new TestObject())
                , clientId);

            _transactionManager.EndTransaction(clientId);

            Assert.AreEqual(1, undoStack.UndoStackCount);

        }
        
    }
}