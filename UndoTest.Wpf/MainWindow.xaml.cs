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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private APlayClient _client;
        private bool _canUndo;
        private bool _canRedo;

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
                new ThreadStart(() =>
                {
                    Title = "Undo Redo Test [ClientId: " + NewDataClient__.Id + "]";
                    DataContext = new MainWindowVm(DataClient);
                }));


        }

        public Client DataClient { get; private set; }

        private void CreateSingleActionUndo(object sender, RoutedEventArgs e)
        {

        }

        private void ManualTransactionUndo(object sender, RoutedEventArgs e)
        {

        }

        private void Redo_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowVm) DataContext).Redo();
        }

        private void Undo_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowVm)DataContext).Undo();
        }

        private void StartTransaction_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowVm)DataContext).StartTransaction();
        }

        private void EndTransaction_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowVm)DataContext).EndTransaction();
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CancelTransaction_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowVm)DataContext).CancelTransaction();
        }
    }

  
}
