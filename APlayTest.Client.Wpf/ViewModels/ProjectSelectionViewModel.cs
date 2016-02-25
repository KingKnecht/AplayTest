using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Permissions;
using System.Timers;
using System.Windows.Data;
using APlay.Generated.Intern.Client;
using Caliburn.Micro;
using Reactive.Bindings;

namespace APlayTest.Client.Wpf.ViewModels
{
    public class ProjectSelectionViewModel : Screen
    {
        private readonly ProjectManager _projectManager;

        private string _searchString;
        private IObservableCollection<ProjectDetail> _projectDetails;
        private List<ProjectDetail> _internProjectDetailsList;
        private bool _canNewProject;
        private ProjectDetail _selectedProject;
        private bool _canJoinProject;

        public ProjectSelectionViewModel(ProjectManager projectManager)
        {
            _projectManager = projectManager;

            _projectDetails = new BindableCollection<ProjectDetail>();
            ProjectDetails.AddRange(_projectManager.ProjectDetails);

            _projectManager.PropertyChanged += _projectManager_PropertyChanged;
        }

        void _projectManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CanJoinProject = _projectManager.CanJoinProject;
            CanNewProject = _projectManager.CanCreateProject;

            ProjectDetails.Clear();
            ProjectDetails.AddRange(_projectManager.ProjectDetails);

            _selectedProject = _projectManager.SelectedProject;
            NotifyOfPropertyChange(() => SelectedProject);
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

        private void UpdateCommandStates()
        {
            CanNewProject = !ProjectDetails.Any();
            CanJoinProject = !CanNewProject;
        }

        private void OnGetProjectDetails(ProjectDetailList projectDetail)
        {
            ProjectDetails.Clear();
            ProjectDetails.AddRange(projectDetail);
        }

        private IEnumerable<ProjectDetail> FilterBySearchString(IEnumerable<ProjectDetail> projectDetails)
        {
            return projectDetails.Where(d =>
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    return d.Name.StartsWith(SearchString);
                }

                return true;
            }).ToList();
        }

        public IObservableCollection<ProjectDetail> ProjectDetails
        {
            get { return _projectDetails; }
            set
            {
                if (Equals(value, _projectDetails)) return;
                _projectDetails = value;
                NotifyOfPropertyChange(() => ProjectDetails);
            }
        }

        public bool CanNewProject
        {
            get { return _canNewProject; }
            set
            {
                if (value == _canNewProject) return;
                _canNewProject = value;
                NotifyOfPropertyChange(() => CanNewProject);
            }
        }

        public void NewProject()
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

      
    }
}