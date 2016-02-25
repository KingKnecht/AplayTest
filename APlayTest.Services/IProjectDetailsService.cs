using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APlayTest.Services
{

    
    public interface IProjectDetailsService
    {
        IEnumerable<ProjectDetail> GetProjectDetails();
        ProjectDetail CreateProject(string name);
    }

    public class ProjectDetailsService : IProjectDetailsService
    {
        private List<ProjectDetail> _projects = new List<ProjectDetail>();
        private int _nextProjectId;

        public ProjectDetailsService()
        {
            for (int i = 0; i < 5; i++)
            {
                CreateProject("Dummy_" + 1);
            }
        }

        public IEnumerable<ProjectDetail> GetProjectDetails()
        {
            return _projects;
            //for (int i = 0; i < 5; i++)
            //{
            //    yield return new ProjectDetail()
            //    {
            //        CreatedBy = "Sven Bardos",
            //        CreationDate = DateTime.Now,
            //        Name = "Project_" + i,
            //        ProjectId = i
            //    };
            //}
        }

        public ProjectDetail CreateProject(string name)
        {

            var newProjectDetails = new ProjectDetail()
            {
                CreatedBy = "Not impl.",
                CreationDate = DateTime.Now,
                Name = name,
                ProjectId = ++_nextProjectId
            };

            _projects.Add(newProjectDetails);

            return newProjectDetails;
        }
    }

    public class ProjectDetail
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int ProjectId { get; set; }
    }
}
