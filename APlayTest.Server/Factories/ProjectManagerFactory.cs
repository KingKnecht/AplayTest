using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services;

namespace APlayTest.Server.Factories
{

    public class ProjectFactory : IProjectFactory
    {
        private readonly IProjectService _projectService;

        public ProjectFactory(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public Project CreateProject()
        {


            //aplayProject.ProjectDetail = new ProjectDetail(srvProject.ProjectDetail.Name,
            //    srvProject.ProjectDetail.CreatedBy, srvProject.ProjectDetail.CreationDate,
            //    srvProject.ProjectDetail.ProjectId);
            throw new NotImplementedException();
        }
    }

    public class ProjectManagerFactory : IProjectManagerFactory
    {
        private readonly IProjectManagerService _projectManagerService;
        private readonly AplayProjectsCache _aplayProjectsCache;

        public ProjectManagerFactory(IProjectManagerService projectManagerService)
        {
            _projectManagerService = projectManagerService;
            _aplayProjectsCache = new AplayProjectsCache();
        }

        public ProjectManager CreateProjectManager()
        {
            return new ProjectManager(_projectManagerService, _aplayProjectsCache);
        }
    }
}
