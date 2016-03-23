using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace sbardos.UndoFramework.Tests
{
    [TestClass]
    public class UndoStackTests
    {
        private UndoStack _undoStack;

        [TestInitialize]
        public void Init()
        {
            _undoStack = new UndoStack(666);
        }


        [TestMethod]
        public void InitTest()
        {

            Assert.IsFalse(_undoStack.CanUndo);
            Assert.IsFalse(_undoStack.CanRedo);
            Assert.AreEqual(0, _undoStack.UndoStackCount);
            Assert.AreEqual(0, _undoStack.RedoStackCount);
        }

        [TestMethod]
        public void PushTest()
        {
            _undoStack.Push(new ChangeSet(0, 1));

            Assert.IsTrue(_undoStack.CanUndo);
            Assert.IsFalse(_undoStack.CanRedo);
            Assert.AreEqual(1, _undoStack.UndoStackCount);
            Assert.AreEqual(0, _undoStack.RedoStackCount);
        }

        [TestMethod]
        public void Undo_Over_Limit_Test()
        {
            _undoStack.Undo(); 
           
            var cs = _undoStack.GetActiveChangeSet();

            Assert.IsNotNull(cs);
            Assert.IsFalse(_undoStack.CanUndo);
            Assert.IsFalse(_undoStack.CanRedo);
            Assert.AreEqual(0, _undoStack.UndoStackCount);
            Assert.AreEqual(0, _undoStack.RedoStackCount);
        }

        [TestMethod]
        public void Redo_Over_Limit_Test()
        {
            _undoStack.Redo();
            
            Assert.IsFalse(_undoStack.CanUndo);
            Assert.IsFalse(_undoStack.CanRedo);
            Assert.AreEqual(0, _undoStack.UndoStackCount);
            Assert.AreEqual(0, _undoStack.RedoStackCount);
        }

        [TestMethod]
        public void Push_Undo_Test()
        {
            _undoStack.Push(new ChangeSet(0, 1));
            _undoStack.Undo();

            var cs = _undoStack.GetActiveChangeSet();

            Assert.IsNotNull(cs);
            Assert.IsFalse(_undoStack.CanUndo);
            Assert.IsTrue(_undoStack.CanRedo);
            Assert.AreEqual(0, _undoStack.UndoStackCount);
            Assert.AreEqual(1, _undoStack.RedoStackCount);
        }

        [TestMethod]
        public void Push_Undo_Redo_Test()
        {
            _undoStack.Push(new ChangeSet(0, 1));

            var cs1 = _undoStack.GetActiveChangeSet();
      
            _undoStack.Undo();
            
            _undoStack.Redo();
            var cs2 = _undoStack.GetActiveChangeSet();
      
            Assert.AreSame(cs1, cs2);
            
            Assert.IsTrue(_undoStack.CanUndo);
            Assert.IsFalse(_undoStack.CanRedo);
            Assert.AreEqual(1, _undoStack.UndoStackCount);
            Assert.AreEqual(0, _undoStack.RedoStackCount);
        }

        [TestMethod]
        public void Prune_RedoStack_Test()
        {
            _undoStack.Push(new ChangeSet(0, 1));
            _undoStack.Undo();

            _undoStack.Push(new ChangeSet(0, 1)); //This should delete the redo stack.
            
            Assert.IsTrue(_undoStack.CanUndo);
            Assert.IsFalse(_undoStack.CanRedo);
            Assert.AreEqual(1, _undoStack.UndoStackCount);
            Assert.AreEqual(0, _undoStack.RedoStackCount);
        }
    }
}
