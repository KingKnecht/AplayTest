using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Caliburn.Micro;
using APlay.Client;
using Reactive.Bindings;
using Reactive.Bindings.Interactivity;

namespace APlayTest.Client.Wpf.ViewModels
{

    public interface IShellViewModel
    {

    }

    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShellViewModel, IDisposable
    {
        private readonly IWindowManager _windowManager;
        public static APlayClient APlayClient;
        private CompositeDisposable _cleanup;
        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            APlayClient = new APlayClient();

             APlayClient.Start("127.0.0.1:63422");

            var connectObservable = Observable.FromEvent<Delegates.void_Client, Client>(
                ev => APlayClient.ConnectEventHandler += ev,
                ev => APlayClient.ConnectEventHandler -= ev)
                .Subscribe(client =>
                {
                    APlayClient.DataClient = client;

                    APlayClient.DataClient.CurrentUser = new User()
                    {
                        Name = Environment.UserName
                    };

                    if (APlayClient.DataClient.CurrentProject == null)
                    {
                        if (APlayClient.DataClient.ProjectManager != null)
                        {
                            ActivateItem(new ProjectSelectionViewModel(APlayClient.DataClient.ProjectManager));
                        }
                    }
                });

            _cleanup = new CompositeDisposable(connectObservable);
        }

        public void Dispose()
        {
            _cleanup.Dispose();
        }
    }
}
