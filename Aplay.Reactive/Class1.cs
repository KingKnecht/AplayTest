using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;

namespace Aplay.Reactive
{

    public class AplaySourceList<T> : ISourceList<T>
    {
        private SourceList<T> _sourceList;

        public AplaySourceList(EventHandler addAction)
        {
            _sourceList = new SourceList<T>();

            //addAction +=(sender, args) => {_sourceList.Add();}
        }

        public void Edit(Action<IExtendedList<T>> updateAction, Action<Exception> errorHandler = null)
        {
            _sourceList.Edit(updateAction,errorHandler);
        }

        public void Dispose()
        {
            _sourceList.Dispose();
        }

        public IObservable<IChangeSet<T>> Connect(Func<T, bool> predicate = null)
        {
            return _sourceList.Connect(predicate);
        }

        public IObservable<int> CountChanged { get; private set; }
        public IEnumerable<T> Items { get; private set; }
        public int Count { get; private set; }
    }

    public static class AplayReactive
    {
        public static IObservableList<T> ToReactiveSourceList<T>(this IList<T> list, Action<T> addAction,Action<T> removeAction )
        {
            var sourceList = new SourceList<T>();

           

            return sourceList;
        }
    }
}
