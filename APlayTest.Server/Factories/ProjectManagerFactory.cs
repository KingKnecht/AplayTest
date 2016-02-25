using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services;

namespace APlayTest.Server.Factories
{
    public class ProjectManagerFactory : IProjectManagerFactory
    {
        private readonly IProjectDetailsService _projectDetailsService;

        public ProjectManagerFactory(IProjectDetailsService projectDetailsService)
        {
            _projectDetailsService = projectDetailsService;
        }

        public ProjectManager CreateProjectManager()
        {
            return new ProjectManager(_projectDetailsService);
        }
    }
}
