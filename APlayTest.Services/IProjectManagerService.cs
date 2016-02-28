using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services.Annotations;
using APlayTest.Services.Infracstructure;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;

namespace APlayTest.Services
{
    public interface IProjectService
    {

    }

    public interface IProjectManagerService
    {
        IEnumerable<Project> GetProjects(Func<Project, bool> filter);
        Project CreateProject(string projectName, string userName);
        bool IsValidName(string name);

        /// <summary>
        /// Used to notify about changes i.e. project add, removed. Deltas only.
        /// </summary>
        IObservableCache<Project, int> ProjectsDelta { get; }

        Project GetProject(int projectId);
    }



    public class ProjectManagerService : IProjectManagerService, IDisposable
    {
        private readonly List<Project> _projects = new List<Project>();
        private int _nextProjectId;

        private readonly SourceCache<Project, int> _sourceCache = new SourceCache<Project, int>(pd => pd.Id);
        private readonly CompositeDisposable _cleanup = new CompositeDisposable();

        public ProjectManagerService()
        {

            for (int i = 1; i < 5; i++)
            {
                CreateProject("Dummy_" + i, Environment.UserName);
            }

            _cleanup.Add(_sourceCache);
        }


        public IEnumerable<Project> GetProjects(Func<Project, bool> filter)
        {
            return _projects.Where(filter);
        }

        public Project CreateProject(string projectName, string userName)
        {
            //Todo: Lock für Writer; wäre schlecht wenn das 2 Clients/Threads gleichzeitig machen.
            if (!IsValidName(projectName))
            {
                throw new InvalidOperationException("Project name is invalid");//Todo: Etwas heftig hier eine Exception zu werfen. Das muss anders gehen...
            }

            var newProjectDetails = new ProjectDetail()
            {
                CreatedBy = userName,
                CreationDate = DateTime.Now,
                Name = projectName,
            };

            var project = new Project(IdGenerator.GetNextId(), newProjectDetails);

            _projects.Add(project);

            _sourceCache.AddOrUpdate(project);

            return project;
        }

        public bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && _projects.All(pd => pd.ProjectDetail.Name != name);
        }

        public IObservableCache<Project, int> ProjectsDelta
        {
            get { return _sourceCache.AsObservableCache(); }
        }

        public Project GetProject(int projectId)
        {
            return _projects.First(prj => prj.Id == projectId);
        }


        public void Dispose()
        {
            _cleanup.Dispose();
        }
    }

    public class Project
    {


        public Project(int id, ProjectDetail projectDetail)
        {
            ProjectDetail = projectDetail;
            Id = id;
        }

        public int Id { get; private set; }

        public ProjectDetail ProjectDetail { get; private set; }
    }

    public class ProjectDetail
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
