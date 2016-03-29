using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sbardos.UndoFramework.Tests
{
    [TestClass]
    public class ChangeSetTests
    {
        [TestMethod]
        public void AddTest()
        {
            var obj = new TestObject();
            
            var changeSet = new ChangeSet(0, 1)
            {
                new Change(obj.Id, obj),
                new Change(ChangeReason.ObjectUpdate, obj.Id, obj) //Same object added again. No Change.
            };

            Assert.AreEqual(1, changeSet.Count);
            Assert.AreEqual(obj, changeSet[0].UndoObjectState);
        }

        [TestMethod]
        public void OrderTest()
        {
            var obj1 = new TestObject();
            var obj2 = new TestObject();

            var changeSet = new ChangeSet(0, 1)
            {
                new Change(ChangeReason.ObjectUpdate, obj1.Id, obj1),
                new Change(ChangeReason.ObjectUpdate, obj2.Id, obj2)
            };

            Assert.AreEqual(2, changeSet.Count);

        }

        [TestMethod]
        public void OrderTest2()
        {
            var obj1 = new TestObject();
            var obj2 = new TestObject();

            var changeSet = new ChangeSet(0, 1)
            {
                new Change(ChangeReason.ObjectUpdate, obj1.Id, obj1),
                new Change(ChangeReason.ObjectUpdate, obj2.Id, obj2),
                new Change(ChangeReason.ObjectUpdate, obj1.Id, obj1), //This does not update the position of the change!
            };

            Assert.AreEqual(2, changeSet.Count);
            Assert.AreEqual(obj1, changeSet[0].UndoObjectState);
            Assert.AreEqual(obj2, changeSet[1].UndoObjectState);

        }
    }
}