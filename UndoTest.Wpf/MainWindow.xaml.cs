using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APlay.Generated.Intern.Client;
using Undo.Client;
using UndoTest.Wpf.Annotations;

namespace UndoTest.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private APlayClient _client;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _client = new APlayClient();
            _client.Start("127.0.0.1:55555");
            _client.ConnectEventHandler += _client_ConnectEventHandler;


        }

        void _client_ConnectEventHandler(Client NewDataClient__)
        {

            DataClient = NewDataClient__;

            Dispatcher.BeginInvoke(
                new ThreadStart(() => DataContext = new MainWindowVm(DataClient)));

        }

        public Client DataClient { get; private set; }

        private void CreateSingleActionUndo(object sender, RoutedEventArgs e)
        {

        }

        private void ManualTransactionUndo(object sender, RoutedEventArgs e)
        {

        }
    }

    internal class MainWindowVm : INotifyPropertyChanged
    {
        private readonly Client _client;
        private TaskManagerVm _taskManagerVm;
        private HistoryVm _historyVm;

        public MainWindowVm(Client client)
        {
            _client = client;
            TaskManagerVm = new TaskManagerVm(_client.TaskManager, _client);
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


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class HistoryVm
    {

    }
}
