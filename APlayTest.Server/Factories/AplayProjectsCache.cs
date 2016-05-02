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

        private readonly Dictionary<int, UndoManager> _cache = new Dictionary<int, UndoManager>(); 

        public UndoManager GetUndoManager(int clientId)
        {
            UndoManager undoManager;
            if (!_cache.TryGetValue(clientId, out undoManager))
            {
                _cache[clientId] = new UndoManager(_undoService, clientId);
                undoManager = _cache[clientId];
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
        UndoManager GetUndoManager(int clientId);
    }
}