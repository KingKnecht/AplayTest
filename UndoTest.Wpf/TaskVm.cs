using System.ComponentModel;
using System.Runtime.CompilerServices;
using Undo.Client;
using UndoTest.Wpf.Annotations;

namespace UndoTest.Wpf
{
    public class TaskVm : INotifyPropertyChanged
    {
        private readonly Task _task;
        private readonly Client _client;
        private bool _isDone;
        private string _description;

        public TaskVm(Task task, Client client)
        {
            _task = task;
            _client = client;
            IsDone = task.IsDone;
            Description = task.Description;
            Id = task.Id;
            task.DescriptionChangeEventHandler += description => Description = description;
            task.IsDoneChangeEventHandler += done => IsDone = done;
        }

        public int Id { get;private set; }

        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                if (value == _isDone) return;
                _isDone = value;
                _task.SetDone(value, _client);
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                _task.SetTaskDescription(_description);
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