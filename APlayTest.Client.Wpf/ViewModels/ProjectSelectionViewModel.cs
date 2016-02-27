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
using Reactive.Bindings.Binding;
using Reactive.Bindings.Extensions;

namespace APlayTest.Client.Wpf.ViewModels
{
    public class ProjectSelectionViewModel : Screen, IDisposable
    {
        private readonly ProjectManager _projectManager;

        private ProjectDetail _selectedProject;
        private readonly CompositeDisposable _cleanUp;
        private readonly ReadOnlyObservableCollection<ProjectDetail> _projectDetailsRx;

        public ProjectSelectionViewModel(ProjectManager projectManager)
        {
            _projectManager = projectManager;

            var detailsDisp = _projectManager.ProjectDetailsRx.Connect()
             .ObserveOnUIDispatcher()
                .Bind(out _projectDetailsRx)
                .Subscribe();

            CanJoinProjectRx = new ReactiveProperty<bool>();
            CanCreateProjectRx = new ReactiveProperty<bool>();
            SelectedProjectRx = new ReactiveProperty<ProjectDetail>();

            var selectedProjectAction =
                SelectedProjectRx.Subscribe(selected => _projectManager.SelectedProjectRx.Value = selected);

            SearchStringRx = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.DistinctUntilChanged);
             
            var searchAction =  SearchStringRx
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(searchString => _projectManager.SearchProjects(searchString));

            var joinDisp = _projectManager.CanJoinProjectRx.Subscribe(value => CanJoinProjectRx.Value = value);
            var createDisp = _projectManager.CanCreateProjectRx.Subscribe(value => CanCreateProjectRx.Value = value);
            var selectDisp = _projectManager.SelectedProjectRx.Subscribe(value => SelectedProjectRx.Value = value);

            _cleanUp = new CompositeDisposable(joinDisp, createDisp, detailsDisp, selectDisp, searchAction,
                CanJoinProjectRx, SelectedProjectRx, selectedProjectAction);

        }

    
        public ReadOnlyObservableCollection<ProjectDetail> ProjectDetailsRx
        {
            get { return _projectDetailsRx; }
        }

        public ReactiveProperty<ProjectDetail> SelectedProjectRx { get;private set; }
        
        public ReactiveProperty<string> SearchStringRx { get;private set; }
        public ReactiveProperty<bool> CanJoinProjectRx { get; private set; }
        public ReactiveProperty<bool> CanCreateProjectRx { get; private set; }


        public void CreateProject()
        {
            _projectManager.CreateProject(SearchStringRx.Value);
        }

        public void JoinProject()
        {
            _projectManager.JoinProject(SelectedProjectRx.Value.ProjectId);
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}