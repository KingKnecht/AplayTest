using System.ComponentModel;
using System.Runtime.CompilerServices;
using Undo.Client;
using UndoTest.Wpf.Annotations;

namespace UndoTest.Wpf
{
    internal class MainWindowVm : INotifyPropertyChanged
    {
        private readonly Client _client;
        private TaskManagerVm _taskManagerVm;
        private HistoryVm _historyVm;
        private bool _canUndo;
        private bool _canRedo;
        private string _transactionDescription;

        public MainWindowVm(Client client)
        {
            _client = client;

            _client.UndoManager.CanUndoChangeEventHandler += state => CanUndo = state;
            _client.UndoManager.CanRedoChangeEventHandler += state => CanRedo = state;

            TaskManagerVm = new TaskManagerVm(_client.TaskManager, _client);
            HistoryVm = new HistoryVm(_client.UndoManager, _client);

            _transactionDescription = "Description...";
        }

        public TaskManagerVm TaskManagerVm
        {
            get { return _taskManagerVm; }
            set
            {
                if (Equals(value, _taskManagerVm)) return;
                _taskManagerVm = value;
                OnPropertyChanged();
            }
        }

        public string TransactionDescription
        {
            get { return _transactionDescription; }
            set
            {
                if (value == _transactionDescription) return;
                _transactionDescription = value;
                OnPropertyChanged();
            }
        }

        public bool CanUndo
        {
            get { return _canUndo; }
            set
            {
                if (value == _canUndo) return;
                _canUndo = value;
                OnPropertyChanged();
            }
        }

        public bool CanRedo
        {
            get { return _canRedo; }
            set
            {
                if (value == _canRedo) return;
                _canRedo = value;
                OnPropertyChanged();
            }
        }

        public HistoryVm HistoryVm
        {
            get { return _historyVm; }
            set
            {
                if (Equals(value, _historyVm)) return;
                _historyVm = value;
                OnPropertyChanged();
            }
        }


        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public void Redo()
        {
            _client.UndoManager.ExecuteRedo();
        }

        public void Undo()
        {
            _client.UndoManager.ExecuteUndo();
        }

        public void StartTransaction()
        {
            _client.UndoManager.StartTransaction(TransactionDescription == string.Empty ? "Unknown" : TransactionDescription);
        }

        public void EndTransaction()
        {
            _client.UndoManager.EndTransaction();
        }
    }
}