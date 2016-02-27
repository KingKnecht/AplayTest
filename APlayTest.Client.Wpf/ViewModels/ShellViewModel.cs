using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Caliburn.Micro;
using APlay.Client;

namespace APlayTest.Client.Wpf.ViewModels
{

    public interface IShellViewModel
    {

    }

    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShellViewModel
    {
        private readonly IWindowManager _windowManager;
        public static APlayClient APlayClient;

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            APlayClient = new APlayClient();

            APlayClient.ConnectEventHandler += _aPlayClient_ConnectEventHandler;
            APlayClient.DisconnectEventHandler += _aPlayClient_DisconnectEventHandler;

            APlayClient.Start("127.0.0.1:63422");

            
        }

        void _aPlayClient_DisconnectEventHandler()
        {

        }

        void _aPlayClient_ConnectEventHandler(Client clientObject)
        {
            APlayClient.DataClient = clientObject;

            APlayClient.DataClient.CurrentUser = new User()
            {
                Name = Environment.UserName
            };
            
            WeakEventManager<Client, PropertyChangedEventArgs>.AddHandler(APlayClient.DataClient, "PropertyChanged", DataClientChangedHandler);

            if (APlayClient.DataClient.CurrentProject == null)
            {
                if (APlayClient.DataClient.ProjectManager != null)
                {
                    ActivateItem(new ProjectSelectionViewModel(APlayClient.DataClient.ProjectManager));
                }
            }
        }

        private void DataClientChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var client = sender as Client;

            if (client != null)
            {
                if (client.CurrentProject != null)
                {
                    WeakEventManager<Project, PropertyChangedEventArgs>.AddHandler(client.CurrentProject, "PropertyChanged", ProjectChangedHandler);
                    DisplayName = client.CurrentProject.Name;
                }
            }
        }

        private void ProjectChangedHandler(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var project = sender as Project;
            if (project != null)
            {
                DisplayName = project.Name;
            }
        }

    }
}
