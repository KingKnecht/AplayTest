using System.Collections.Generic;

namespace APlayTest.Server.Factories
{


    public class AplayProjectsCache : IAplayProjectsCache
    {
        private readonly Dictionary<int, Project> projects = new Dictionary<int, Project>();
        public bool TryGetProject(int id, out Project project)
        {
            return projects.TryGetValue(id, out project);
        }

        public bool RemoveProject(int id)
        {
           return projects.Remove(id);
        }

        public void AddProject(Project project)
        {
            projects.Add(project.Id, project);
        }
    }

    public interface IAplayProjectsCache
    {
        bool TryGetProject(int id, out Project project);
        bool RemoveProject(int id);

        void AddProject(Project project);
    }
}