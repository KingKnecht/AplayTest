using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Undo.Client;
using UndoTest.Wpf.Annotations;

namespace UndoTest.Wpf
{
    public class TaskManagerVm : INotifyPropertyChanged
    {
        private readonly TaskManager _taskManager;
        private readonly Client _client;
        private ObservableCollection<TaskVm> _tasks;
        private TaskVm _selectedTask;
        public event PropertyChangedEventHandler PropertyChanged;

        public TaskManagerVm(TaskManager taskManager, Client client)
        {
            _taskManager = taskManager;
            _client = client;
            _taskManager.TasksAddEventHandler += _taskManager_TasksAddEventHandler;
            _taskManager.TasksRemoveEventHandler += _taskManager_TasksRemoveEventHandler;
            _taskManager.TasksInsertAtEventHandler += _taskManager_TasksInsertAtEventHandler;
            _taskManager.TasksRemoveAtEventHandler += TaskManagerOnTasksRemoveAtEventHandler;
            Tasks = new ObservableCollection<TaskVm>(_taskManager.Tasks.Select(t => new TaskVm(t, _client)));
        }

        private void TaskManagerOnTasksRemoveAtEventHandler(int index, Task @this)
        {
            Application.Current.Dispatcher.BeginInvoke(
               new ThreadStart(() => Tasks.RemoveAt(index)));
        }

        void _taskManager_TasksInsertAtEventHandler(int index, Task task)
        {
            Application.Current.Dispatcher.BeginInvoke(
               new ThreadStart(() => Tasks.Insert(index,new TaskVm(task, _client))));
        }



        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        void _taskManager_TasksRemoveEventHandler(Task task)
        {

            Application.Current.Dispatcher.BeginInvoke(
                new ThreadStart(() => Tasks.Remove(Tasks.First(t => t.Id == task.Id))));
        }

        void _taskManager_TasksAddEventHandler(Undo.Client.Task newTask)
        {

            Application.Current.Dispatcher.BeginInvoke(
                new ThreadStart(() => Tasks.Add(new TaskVm(newTask, _client))));
        }

        public ObservableCollection<TaskVm> Tasks
        {
            get { return _tasks; }
            set
            {
                if (Equals(value, _tasks)) return;
                _tasks = value;
                OnPropertyChanged();
            }
        }

        public TaskVm SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                if (Equals(value, _selectedTask)) return;
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        public void SetDescription(string description)
        {
            if (_selectedTask != null)
            {
                _selectedTask.Description = description;
            }
        }
        public void AddNewTask()
        {
            _taskManager.CreateTask(_client, OnTaskCreated);

        }

        private void OnTaskCreated(Task newTask)
        {
            _taskManager.AddTask(newTask, _client);
        }


        public void RemoveTask()
        {
            if (_selectedTask != null)
            {
                _taskManager.RemoveTask(_selectedTask.Id, _client);
            }
        }
    }
}