using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sbardos.UndoFramework.Tests
{
    [TestClass]
    public class UndoRedoEventsTests
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

            _transactionService.StartTransaction(clientId);

            _transactionService.Add(
                new Change(ChangeReason.InsertAt, 1, new TestObject())
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

            _transactionService.StartTransaction(clientId);

            _transactionService.Add(
                new Change(ChangeReason.InsertAt, 1, new TestObject())
                , clientId);

            _transactionService.EndTransaction(clientId);

            Assert.AreEqual(1, eventsRaised);
        }

    }
}