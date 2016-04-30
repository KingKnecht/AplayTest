using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using APlayTest.Client;
using APlayTest.Client.Annotations;
using APlayTest.Client.Contracts;
using Gemini.Framework;
using Gemini.Framework.Services;


namespace AplayTest.Client.Modules.UndoHistory.ViewModels
{

    public interface IUndoHistoryViewModel : ITool
    {
        ObservableCollection<HistoryEntry> History { get; set; }
    }

    
    [Export(typeof(UndoHistoryViewModel))]
    public class UndoHistoryViewModel : Tool, IUndoHistoryViewModel
    {
        private readonly IAPlayAwareShell _shell;
        private int _selectedIndex;
        private PaneLocation _preferredLocation = PaneLocation.Right;

        [ImportingConstructor]
        public UndoHistoryViewModel(IAPlayAwareShell shell)
        {
            _shell = shell;
            DisplayName = "Undo/Redo History";
            
            if (_shell.UndoManager != null)
            {
                _shell.UndoManager.HistoryAddEventHandler += UndoManagerOnHistoryAddEventHandler;
                _shell.UndoManager.HistoryRemoveAtEventHandler += UndoManagerOnHistoryRemoveAtEventHandler;
                _shell.UndoManager.ActiveHistoryEntryIdChangeEventHandler += _undoManager_ActiveHistoryEntryIdChangeEventHandler;
                _shell.UndoManager.HistoryRemoveEventHandler += _undoManager_HistoryRemoveEventHandler;
                
                History =
                    new ObservableCollection<HistoryEntry>(
                        _shell.UndoManager.History.Select(item => new HistoryEntry(item.Id, item.Description)));

            }
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
                new ThreadStart(() => History.RemoveAt(pos)));
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

        public override PaneLocation PreferredLocation
        {
            get { return _preferredLocation; }
        }
    }
}
