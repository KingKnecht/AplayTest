﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using APlayTest.Client.Contracts;
using APlayTest.Client.Factories;
using APlayTest.Client.Gemini.Properties;
using Gemini.Framework.Services;

namespace APlayTest.Client.Gemini.MainWindow.ViewModels
{
    interface IMainWindowEx : IMainWindow
    {
        JoinProjectViewModel JoinProjectViewModel { get; }
        ServerNotFoundViewModel ServerNotFoundViewModel { get; }
    }

    [Export(typeof(IMainWindowEx))]
    public class MainWindowViewModel : global::Gemini.Modules.MainWindow.ViewModels.MainWindowViewModel, IMainWindowEx, IPartImportsSatisfiedNotification
    {
        private readonly IAplayClientFactory _aplayClientFactory;
        private APlayClient _aplayClient;
        private readonly string _serverAddress;
        private ServerNotFoundViewModel _serverNotFoundViewModel;
        private JoinProjectViewModel _joinProjectViewModel;
        private IAPlayAwareShell _aPlayAwareShell;

        [ImportingConstructor]
        public MainWindowViewModel(IAplayClientFactory aplayClientFactory)
        {
            Title = "APlayTest [not joined to project]";
            _aplayClientFactory = aplayClientFactory;
            _serverAddress = Properties.Settings.Default.ServerAddress;
            StartNewClient();
            Width = 1200;
            Icon = null;
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            Icon = null;
            //if (_icon == null)
                //_resourceManager.GetBitmap("Resources/Icons/Gemini-32.png");
            ActivateItem(Shell);
        }

        private void StartNewClient()
        {
            _aplayClient = _aplayClientFactory.Create();
            
            _aplayClient.ConnectionFailedEventHandler += aplayClient_ConnectionFailedEventHandler;
            _aplayClient.ConnectEventHandler += _aplayClient_ConnectEventHandler;
            _aplayClient.DisconnectEventHandler += _aplayClient_DisconnectEventHandler;
            

            _aplayClient.Start(_serverAddress);
        }

        void _aplayClient_DisconnectEventHandler()
        {

            Settings.Default.LastClientId = _aplayClient.DataClient.Id;
            Settings.Default.Save();

            //if (ServerNotFoundViewModel == null)
            //{
            //    ServerNotFoundViewModel = new ServerNotFoundViewModel();
            //}

            RemoveOldHandlers();

            //StartNewClient();
        }

        private void RemoveOldHandlers()
        {
            _aplayClient.ConnectionFailedEventHandler -= aplayClient_ConnectionFailedEventHandler;
            _aplayClient.ConnectEventHandler -= _aplayClient_ConnectEventHandler;
            _aplayClient.DisconnectEventHandler -= _aplayClient_DisconnectEventHandler;
            _aplayClient.DataClient.CurrentProjectChangeEventHandler -= DataClientOnCurrentProjectChangeEventHandler;
        }

        void _aplayClient_ConnectEventHandler(Client NewDataClient__)
        {
            if (Settings.Default.LastClientId > 0 && NewDataClient__.Id != Settings.Default.LastClientId) 
            {
                NewDataClient__.TryGetId(Settings.Default.LastClientId, NewDataClient__, value => {;});    
            }
            
            ServerNotFoundViewModel = null;

            _aplayClient.DataClient = NewDataClient__;
            
            _aplayClient.DataClient.CurrentUser = new User() { Name = Environment.UserName };

            var aPlayAwareShell = (IAPlayAwareShell)Shell;
            aPlayAwareShell.Client = NewDataClient__;
            
            JoinProjectViewModel = new JoinProjectViewModel(_aplayClient.DataClient.ProjectManager, Close);

            _aplayClient.DataClient.CurrentProjectChangeEventHandler += DataClientOnCurrentProjectChangeEventHandler;
           
        }
        
        private void DataClientOnCurrentProjectChangeEventHandler(Project newCurrentProject)
        {
            _aPlayAwareShell = (IAPlayAwareShell)Shell;

            Title = "APlayTest [" + newCurrentProject.ProjectDetail.Name + "]";
            _aPlayAwareShell.Project = newCurrentProject;
        }

        void aplayClient_ConnectionFailedEventHandler()
        {
            if (ServerNotFoundViewModel == null)
            {
                ServerNotFoundViewModel = new ServerNotFoundViewModel();
            }

            Thread.Sleep(2000);
            _aplayClient.Start(_serverAddress);
        }
        private void Close(IDisposable vm)
        {
            JoinProjectViewModel = null;
            vm.Dispose();
        }
        public JoinProjectViewModel JoinProjectViewModel
        {
            get { return _joinProjectViewModel; }
            private set
            {
                if (Equals(value, _joinProjectViewModel)) return;
                _joinProjectViewModel = value;
                NotifyOfPropertyChange(() => JoinProjectViewModel);
            }
        }

        public ServerNotFoundViewModel ServerNotFoundViewModel
        {
            get { return _serverNotFoundViewModel; }
            private set
            {
                if (Equals(value, _serverNotFoundViewModel)) return;
                _serverNotFoundViewModel = value;
                NotifyOfPropertyChange(() => ServerNotFoundViewModel);
            }
        }
    }
}
