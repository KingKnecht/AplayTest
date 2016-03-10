using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace APlayTest.Client.Gemini.MainWindow.ViewModels
{
   
    public class JoinProjectViewModel : IDisposable
    {
        private readonly ProjectManager _projectManager;
        private readonly CompositeDisposable _cleanUp;
        private readonly ReadOnlyObservableCollection<ProjectDetailsVm> _projectDetailsRx;


        public JoinProjectViewModel(ProjectManager projectManager, Action<IDisposable> close)
        {
            _projectManager = projectManager;
            
            var detailsDisp = _projectManager.ProjectsRx.Connect()
                .Transform(prj => new ProjectDetailsVm()
                {
                    Name = prj.ProjectDetail.Name,
                    CreationDate = prj.ProjectDetail.CreationDate,
                    CreatedBy = prj.ProjectDetail.CreatedBy,
                    ProjectId = prj.Id
                })
                .ObserveOnUIDispatcher()
                .Bind(out _projectDetailsRx)
                .Subscribe();

            CanJoinProjectRx = new ReactiveProperty<bool>();
            CanCreateProjectRx = new ReactiveProperty<bool>();
            SelectedProjectRx = new ReactiveProperty<ProjectDetailsVm>();

            SearchStringRx = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.DistinctUntilChanged);

            var searchAction = SearchStringRx
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(searchString => _projectManager.SearchProjects(searchString));

            SelectedProjectRx = Observable.FromEvent<Delegates.void_Project, Project>(
                ev => _projectManager.SelectedProjectChangeEventHandler += ev,
                ev => _projectManager.SelectedProjectChangeEventHandler -= ev)
                .Where(project =>
                {
                    if (project == null)
                        return false;

                    if (SelectedProjectRx.Value != null && project.Id == SelectedProjectRx.Value.ProjectId)
                        return false;

                    return true;
                })
                .Select(
                    prj =>
                    {
                        if (prj == null)
                        {
                            return null;
                        }

                        return new ProjectDetailsVm()
                        {
                            Name = prj.ProjectDetail.Name,
                            CreationDate = prj.ProjectDetail.CreationDate,
                            CreatedBy = prj.ProjectDetail.CreatedBy,
                            ProjectId = prj.Id
                        };

                    })
                .ToReactiveProperty(null, ReactivePropertyMode.DistinctUntilChanged);

            var selectProjectAction = SelectedProjectRx
                .Where(projectDetailsVm => projectDetailsVm != null)
                .Subscribe(pd =>
                {
                    Console.WriteLine("ProjectID selected: " + pd.ProjectId);
                    _projectManager.SelectProject(pd.ProjectId);
                }); //Rückkanal Selektion in der GUI -> Server.

            CanJoinProjectRx = Observable.FromEvent<Delegates.void_boolean, bool>(
                ev => _projectManager.CanJoinProjectChangeEventHandler += ev,
                ev => _projectManager.CanJoinProjectChangeEventHandler -= ev)
                .ToReactiveProperty();

            CanCreateProjectRx = Observable.FromEvent<Delegates.void_boolean, bool>(
                ev => _projectManager.CanCreateProjectChangeEventHandler += ev,
                ev => _projectManager.CanCreateProjectChangeEventHandler -= ev)
                .ToReactiveProperty();

            var joinedProjectObservable = Observable.FromEvent<Delegates.void_Project, Project>(
                ev => _projectManager.JoinedProjectEventHandler += ev,
                ev => _projectManager.JoinedProjectEventHandler -= ev)
                .Subscribe(
                    prj =>
                    {
                        //Logger.LogDesigned(2,
                        //    "'Joined project: " + prj.ProjectDetail.Name,
                        //    "Client.Designed");

                        close(this);
                    }
                );

            _cleanUp = new CompositeDisposable(selectProjectAction, CanCreateProjectRx, CanJoinProjectRx, detailsDisp, searchAction,
                SelectedProjectRx, joinedProjectObservable);
        }

        public ReadOnlyObservableCollection<ProjectDetailsVm> ProjectDetailsRx
        {
            get { return _projectDetailsRx; }
        }

        public ReactiveProperty<ProjectDetailsVm> SelectedProjectRx { get; private set; }

        public ReactiveProperty<string> SearchStringRx { get; private set; }
        public ReactiveProperty<bool> CanJoinProjectRx { get; private set; }
        public ReactiveProperty<bool> CanCreateProjectRx { get; private set; }


        public void CreateProject()
        {
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

    public class ProjectDetailsVm
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int ProjectId { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}][{1}][{2}][{3}]", Name, CreatedBy, CreationDate, ProjectId);
        }

        protected bool Equals(ProjectDetailsVm other)
        {
            return ProjectId == other.ProjectId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectDetailsVm)obj);
        }

        public override int GetHashCode()
        {
            return ProjectId;
        }
    }
}
