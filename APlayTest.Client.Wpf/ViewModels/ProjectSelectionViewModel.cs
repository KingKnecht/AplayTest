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

            SearchStringRx = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.DistinctUntilChanged);

            var searchAction = SearchStringRx
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(searchString => _projectManager.SearchProjects(searchString));

            SelectedProjectRx = Observable.FromEvent<Delegates.void_ProjectDetail, ProjectDetail>(
                ev => _projectManager.SelectedProjectDetailChangeEventHandler += ev,
                ev => _projectManager.SelectedProjectDetailChangeEventHandler -= ev)
                .ToReactiveProperty();
           var selectProjectDetailAction = SelectedProjectRx.Subscribe(pd => _projectManager.SelectProject(pd.ProjectId)); //Rückkanal

            CanJoinProjectRx = Observable.FromEvent<Delegates.void_boolean, bool>(
                ev => _projectManager.CanJoinProjectChangeEventHandler += ev,
                ev => _projectManager.CanJoinProjectChangeEventHandler -= ev)
                .ToReactiveProperty();

            CanCreateProjectRx = Observable.FromEvent<Delegates.void_boolean, bool>(
                ev => _projectManager.CanCreateProjectChangeEventHandler += ev,
                ev => _projectManager.CanCreateProjectChangeEventHandler -= ev)
                .ToReactiveProperty();

            _cleanUp = new CompositeDisposable(selectProjectDetailAction,CanCreateProjectRx, CanJoinProjectRx, detailsDisp, searchAction,
                SelectedProjectRx);

        }


        public ReadOnlyObservableCollection<ProjectDetail> ProjectDetailsRx
        {
            get { return _projectDetailsRx; }
        }

        public ReactiveProperty<ProjectDetail> SelectedProjectRx { get; private set; }

        public ReactiveProperty<string> SearchStringRx { get; private set; }
        public ReactiveProperty<bool> CanJoinProjectRx { get; private set; }
        public ReactiveProperty<bool> CanCreateProjectRx { get; private set; }


        public void CreateProject()
        {
            //Todo: Cleanup
            //Muss das mit _projectManager.DataClient wirklich sein? Das ist ziemlich viel Wissen an der Stelle über PrjMgr.
            //besser überladene Methode und ProjectManager stopft den Client dann selber rein...Problem: Die Methode ist trotzdem sichtbar. IFace?
            _projectManager.CreateProject(_projectManager.DataClient, SearchStringRx.Value);
        }

        public void JoinProject()
        {
            _projectManager.JoinProject(_projectManager.DataClient, SelectedProjectRx.Value.ProjectId);
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}