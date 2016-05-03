using System.Collections.Generic;
using sbardos.UndoFramework;

namespace APlayTest.Server.Factories
{

    public class UndoManagerCache : IUndoManagerCache
    {
        private readonly IUndoService _undoService;
        
        public UndoManagerCache(IUndoService undoService)
        {
            _undoService = undoService;
        }

        //Dictionary<ClientId, Dictionary<ProjectId, UndoManager>>
        private readonly Dictionary<int, Dictionary<int, UndoManager>> _cache = new Dictionary<int, Dictionary<int, UndoManager>>();

        public UndoManager GetUndoManager(int clientId, int projectId)
        {
            Dictionary<int, UndoManager> projectsToUndoManager;
            UndoManager undoManager;

            if (_cache.TryGetValue(clientId, out projectsToUndoManager))
            {
                if (projectsToUndoManager.TryGetValue(projectId, out undoManager)) 
                    return undoManager;

                _cache[clientId] = new Dictionary<int, UndoManager>();
                _cache[clientId][projectId] = new UndoManager(_undoService, clientId);
                undoManager = _cache[clientId][projectId];
            }
            else
            {
                _cache[clientId] = new Dictionary<int, UndoManager>();
                _cache[clientId][projectId] = new UndoManager(_undoService, clientId);
                undoManager = _cache[clientId][projectId];
            }

            return undoManager;
        }

      
    }


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

    public interface IUndoManagerCache
    {
        UndoManager GetUndoManager(int clientId, int projectId);
    }
}