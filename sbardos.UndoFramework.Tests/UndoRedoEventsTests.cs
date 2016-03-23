using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sbardos.UndoFramework.Tests
{
    [TestClass]
    public class UndoRedoEventsTests
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
        public void No_EndTransaction_EventsRaisedTest()
        {
            var clientId = 101;

             _undoStackManager.CreateStackForClient(clientId);
            var undoStack = _undoStackManager.GetStackOfClient(clientId);

            int eventsRaised = 0;
            undoStack.ActiveStateChanged += (sender, changeSet) =>
            {
                eventsRaised++;
            };

            _transactionManager.StartTransaction(clientId);

            _transactionManager.Add(
                new Change(ChangeReason.Add, 1, new TestObject())
                , clientId);
            

            Assert.AreEqual(0, eventsRaised);
        }

        [TestMethod]
        public void EventsRaisedTest()
        {
            var clientId = 101;

             _undoStackManager.CreateStackForClient(clientId);
             var undoStack = _undoStackManager.GetStackOfClient(clientId);

            int eventsRaised = 0;
            undoStack.ActiveStateChanged += (sender, changeSet) =>
            {
                eventsRaised++;
            };

            _transactionManager.StartTransaction(clientId);

            _transactionManager.Add(
                new Change(ChangeReason.Add, 1, new TestObject())
                , clientId);

            _transactionManager.EndTransaction(clientId);

            Assert.AreEqual(1, eventsRaised);
        }

    }
}