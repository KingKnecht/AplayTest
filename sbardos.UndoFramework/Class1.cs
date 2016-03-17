using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using sbardos.UndoFramework.Annotations;

namespace sbardos.UndoFramework
{
    public interface IComponent
    {
        int Id { get; }
        IComponent UndoClone();
    }

    public class UndoStack :INotifyPropertyChanged
    {
        private readonly int _maxStackSize;
        private readonly C5.LinkedList<ChangeSet> _undoStack = new C5.LinkedList<ChangeSet>();
        private readonly C5.LinkedList<ChangeSet> _redoStack = new C5.LinkedList<ChangeSet>();
        public event PropertyChangedEventHandler PropertyChanged;




        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class ChangeSet
    {
        private readonly C5.LinkedList<IChange> _undoStack = new C5.LinkedList<IChange>();
        private readonly C5.LinkedList<IChange> _redoStack = new C5.LinkedList<IChange>();
    }

    internal interface IChange
    {
        ChangeReason ChangeReason { get; set; }
        int OwnerId { get; set; }
        T GetStoredObject<T>() where T: IComponent;
    }

    public enum ChangeReason
    {
        /// <summary>
        ///  An item has been added
        /// </summary>
        Add,

        /// <summary>
        ///  An item has been updated
        /// </summary>
        Update,

        /// <summary>
        ///  An item has removed
        /// </summary>
        Remove,

        /// <summary>
        /// An item has been moved in a sorted collection
        /// </summary>
        Moved,

    }
}
