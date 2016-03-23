using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using Undo.Client;
using UndoTest.Wpf.Annotations;

namespace UndoTest.Wpf
{
    public class HistoryVm : INotifyPropertyChanged
    {
        private readonly UndoManager _undoManager;
        private readonly Client _client;
        private int _selectedIndex;

        public HistoryVm(UndoManager undoManager, Client client)
        {
            _undoManager = undoManager;
            _client = client;

            _undoManager.HistoryAddEventHandler += UndoManagerOnHistoryAddEventHandler;
            _undoManager.HistoryRemoveAtEventHandler += UndoManagerOnHistoryRemoveAtEventHandler;
            _undoManager.ActiveHistoryEntryIdChangeEventHandler += _undoManager_ActiveHistoryEntryIdChangeEventHandler;
            _undoManager.HistoryRemoveEventHandler += _undoManager_HistoryRemoveEventHandler;
            
            History =
                new ObservableCollection<HistoryEntry>(
                    _undoManager.History.Select(item => new HistoryEntry(item.Id, item.Description)));
        }

        void _undoManager_HistoryRemoveEventHandler(HistoryEntry element)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new ThreadStart(() => History.Remove(element)));  
        }

        void _undoManager_ActiveHistoryEntryIdChangeEventHandler(int NewId__)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new ThreadStart(() =>
                {
                    var activeItem = History.FirstOrDefault(he => he.Id == NewId__);
                    var activeIndex = History.IndexOf(activeItem);
                    SelectedIndex = activeIndex;
                }));  
        }

        private void UndoManagerOnHistoryRemoveAtEventHandler(int pos, HistoryEntry element)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new ThreadStart(() =>   History.RemoveAt(pos)));  
        }

        private void UndoManagerOnHistoryAddEventHandler(HistoryEntry element)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new ThreadStart(() =>
                {
                    History.Add(new HistoryEntry(element.Id, element.Description));
                }));
        }

        public ObservableCollection<HistoryEntry> History { get; set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value == _selectedIndex) return;
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}