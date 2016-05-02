using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services;
using sbardos.UndoFramework;

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
        private readonly IUndoService _undoService;
        private readonly IUndoManagerCache _undoManagerCache;
        private readonly AplayProjectsCache _aplayProjectsCache;

        public ProjectManagerFactory(IProjectManagerService projectManagerService, IUndoService undoService, IUndoManagerCache undoManagerCache)
        {
            _projectManagerService = projectManagerService;
            _undoService = undoService;
            _undoManagerCache = undoManagerCache;
            _aplayProjectsCache = new AplayProjectsCache();
        }

        public ProjectManager CreateProjectManager()
        {
            return new ProjectManager(_projectManagerService, _aplayProjectsCache, _undoService, _undoManagerCache);
        }
    }
}
