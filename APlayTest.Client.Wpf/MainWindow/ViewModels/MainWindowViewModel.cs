using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using APlayTest.Client.Wpf.Framework.Services;
using APlayTest.Client.Wpf.ProjectSelection.ViewModels;
using APlayTest.Client.Wpf.Shell.ViewModels;
using Caliburn.Micro;

namespace APlayTest.Client.Wpf.MainWindow.ViewModels
{

    public class MainWindowViewModel : Conductor<IScreen>, IMainWindow, IDisposable
    {
        private IWindowManager _windowManager;

        private readonly CompositeDisposable _cleanup;
        private IScreen _projectSelectionViewModel;
        private readonly APlayClient _aPlayClient;

        public MainWindowViewModel(IWindowManager windowManager)
        {

            Width = 1000.0;
            Height = 800.0;
            Title = "APlayTest";

            _windowManager = windowManager;
            _aPlayClient = new APlayClient();

            _aPlayClient.Start("127.0.0.1:63422");

            ActivateItem(new ShellViewModel());

            var connectObservable = Observable.FromEvent<Delegates.void_Client, Client>(
                ev => _aPlayClient.ConnectEventHandler += ev,
                ev => _aPlayClient.ConnectEventHandler -= ev)
                .Subscribe(client =>
                {
                    _aPlayClient.DataClient = client;

                    CreateUser();

                    ActivateCreateProjectSelectionVm();          
                });

            _cleanup = new CompositeDisposable(connectObservable);
        }

        public sealed override void ActivateItem(IScreen item)
        {
            base.ActivateItem(item);
        }


        public IScreen ProjectSelectionViewModel
        {
            get { return _projectSelectionViewModel; }
            set
            {
                if (Equals(value, _projectSelectionViewModel)) return;
                _projectSelectionViewModel = value;
                NotifyOfPropertyChange(() => ProjectSelectionViewModel);
            }
        }

        private void ActivateCreateProjectSelectionVm()
        {
            if (_aPlayClient.DataClient.CurrentProject == null)
            {
                if (_aPlayClient.DataClient.ProjectManager != null)
                {
                    var vm = new ProjectSelectionViewModel(_aPlayClient.DataClient.ProjectManager, Close);
                  
                    ProjectSelectionViewModel = vm;
                }
            }
        }

        private void Close(IDisposable toBeDisposed)
        {
            ProjectSelectionViewModel = null;
            toBeDisposed.Dispose();
        }

        private void CreateUser()
        {
            _aPlayClient.DataClient.CurrentUser = new User()
            {
                Name = Environment.UserName
            };
        }

        public void Dispose()
        {
            _cleanup.Dispose();
        }

        public WindowState WindowState { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Title { get; set; }
        public ImageSource Icon { get; set; }
        public IShell Shell { get; private set; }
        public void CloseSpecialControls(object toBeClosed)
        {
            throw new NotImplementedException();
        }
    }
}
