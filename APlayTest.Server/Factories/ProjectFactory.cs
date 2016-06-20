using System;
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
            return new Project();
        }
    }
}