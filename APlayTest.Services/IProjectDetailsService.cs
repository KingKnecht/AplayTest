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
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;

namespace APlayTest.Services
{


    public interface IProjectDetailsService
    {
        IEnumerable<ProjectDetail> GetProjectDetails(Func<ProjectDetail, bool> filter);
        ProjectDetail CreateProject(string projectName, string userName);
        bool IsValidName(string name);

        /// <summary>
        /// Used to notify about changes i.e. project add, removed. Deltas only.
        /// </summary>
        IObservableCache<ProjectDetail, int> ProjectDetailsDelta { get; }

        Project GetProject(int projectId);
    }

    

    public class ProjectDetailsService : IProjectDetailsService, IDisposable
    {
        private readonly List<ProjectDetail> _projects = new List<ProjectDetail>();
        private int _nextProjectId;

        private readonly SourceCache<ProjectDetail, int> _sourceCache = new SourceCache<ProjectDetail, int>(pd => pd.ProjectId);
        private readonly CompositeDisposable _cleanup = new CompositeDisposable();

        public ProjectDetailsService()
        {

            for (int i = 0; i < 5; i++)
            {
                CreateProject("Dummy_" + i, Environment.UserName);
            }

            _cleanup.Add(_sourceCache);
        }


        public IEnumerable<ProjectDetail> GetProjectDetails(Func<ProjectDetail, bool> filter)
        {
            return _projects.Where(filter);
        }

        public ProjectDetail CreateProject(string projectName, string userName)
        {
            //Todo: Lock für Writer; wäre schlecht wenn das 2 Clients/Threads gleichzeitig machen.
            if (!IsValidName(projectName))
            {
                return new ProjectDetail(); //todo: Geheimwissen: Alle ProjectDetails mit ProjectId == 0 werden als "nix" behandelt. Dringend ändern!? vlt doch Klasse statt Struct?
            }

            var newProjectDetails = new ProjectDetail()
            {
                CreatedBy = userName,
                CreationDate = DateTime.Now,
                Name = projectName,
                ProjectId = ++_nextProjectId //Todo: thread-safer Id-Generator muss her.
            };

            _projects.Add(newProjectDetails);

            _sourceCache.AddOrUpdate(newProjectDetails);

            return newProjectDetails;
        }

        public bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && _projects.All(pd => pd.Name != name);
        }

        public IObservableCache<ProjectDetail, int> ProjectDetailsDelta
        {
            get { return _sourceCache.AsObservableCache(); }
        }

        public Project GetProject(int projectId)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            _cleanup.Dispose();
        }
    }

    public class Project
    {
        public Project(ProjectDetail projectDetail)
        {
            ProjectDetail = projectDetail;
        }

        public ProjectDetail ProjectDetail { get;private set; }
    }

    public class ProjectDetail
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int ProjectId { get; set; }
    }
}
