using System.Windows.Media.Animation;
using sbardos.UndoFramework;

namespace APlayTest.Server.Factories
{

    public interface ISheetHostedObjectFactory<out TResult, in TUndoable> where TUndoable : IUndoable
    {
        TResult Create(Sheet sheet);
        TResult Create(int id, Sheet sheet);
        TResult Create(int id, ChangeSet changeSet, Sheet sheet);
        TResult Create(TUndoable undoable, ChangeSet changeSet);
        void Remove(int id);
    }

    public interface IObjectFactory<out TResult, in TUndoable> where TUndoable : IUndoable
    {
        TResult Create();
        TResult Create(int id);
        TResult Create(int id, ChangeSet changeSet);
        TResult Create(TUndoable undoable, ChangeSet changeSet);
        void Remove(int id);
    }
}