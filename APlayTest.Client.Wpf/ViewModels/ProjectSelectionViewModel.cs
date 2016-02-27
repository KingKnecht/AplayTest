using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Permissions;
using System.Timers;
using System.Windows.Data;
using APlay.Generated.Intern.Client;
using Caliburn.Micro;
using DynamicData;
using DynamicData.Binding;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace APlayTest.Client.Wpf.ViewModels
{
    public class ProjectSelectionViewModel : Screen, IDisposable
    {
        private readonly ProjectManager _projectManager;

        private string _searchString;
        private bool _canCreateProject;
        private ProjectDetail _selectedProject;
        private bool _canJoinProject;
        private CompositeDisposable _cleanUp;
        private readonly ReadOnlyObservableCollection<ProjectDetail> _projectDetailsRx;

        public ProjectSelectionViewModel(ProjectManager projectManager)
        {
            _projectManager = projectManager;
        
         
            var detailsDisp = _projectManager.ProjectDetailsRx.Connect()
             .ObserveOnUIDispatcher()
                //.Do(Console.WriteLine)
                .Bind(out _projectDetailsRx)
                .Subscribe();


            var joinDisp = _projectManager.CanJoinProjectRx.Subscribe(value => CanJoinProject = value);
            var createDisp = _projectManager.CanCreateProjectRx.Subscribe(value => CanCreateProject = value);
            var selectDisp = _projectManager.SelectedProjectRx.Subscribe(value => SelectedProject = value);

            _cleanUp = new CompositeDisposable(joinDisp, createDisp, detailsDisp,selectDisp);

        }

        public ReadOnlyObservableCollection<ProjectDetail> ProjectDetailsRx
        {
            get { return _projectDetailsRx; }
        }


        public ProjectDetail SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (value.Equals(_selectedProject)) return;
                _selectedProject = value;

                _projectManager.SelectedProject = value;

                NotifyOfPropertyChange(() => SelectedProject);
            }
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (value == _searchString) return;
                _searchString = value;

                _projectManager.SearchProjects(_searchString);
            }
        }
        

        public bool CanCreateProject
        {
            get { return _canCreateProject; }
            set
            {
                if (value == _canCreateProject) return;
                _canCreateProject = value;
                NotifyOfPropertyChange(() => CanCreateProject);
            }
        }

        public void CreateProject()
        {
            _projectManager.CreateProject(SearchString);
        }

        public bool CanJoinProject
        {
            get { return _canJoinProject; }
            set
            {
                if (value == _canJoinProject) return;
                _canJoinProject = value;
                NotifyOfPropertyChange(() => CanJoinProject);
            }
        }

        public void JoinProject()
        {
            _projectManager.JoinProject(SelectedProject.ProjectId);
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}