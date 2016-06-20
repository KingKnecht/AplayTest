using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services;
using sbardos.UndoFramework;

namespace APlayTest.Server.Factories
{
    public class ProjectManagerFactory : IProjectManagerFactory
    {
        private readonly IProjectManagerService _projectManagerService;
        private readonly IUndoService _undoService;
        private readonly IUndoManagerCache _undoManagerCache;
        private readonly IConnectorFactory _connectorFactory;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IBlockSymbolFactory _blockSymbolFactory;
        private readonly ISheetFactory _sheetFactory;
        private readonly AplayProjectsCache _aplayProjectsCache;

        public ProjectManagerFactory(IProjectManagerService projectManagerService,
            IUndoService undoService, //Todo: Inject a FactoryFactory here?
            IUndoManagerCache undoManagerCache, 
            IConnectorFactory connectorFactory,
            IConnectionFactory connectionFactory,
            IBlockSymbolFactory blockSymbolFactory, 
            ISheetFactory sheetFactory)
        {
            _projectManagerService = projectManagerService;
            _undoService = undoService;
            _undoManagerCache = undoManagerCache;
            _connectorFactory = connectorFactory;
            _connectionFactory = connectionFactory;
            _blockSymbolFactory = blockSymbolFactory;
            _sheetFactory = sheetFactory;
            _aplayProjectsCache = new AplayProjectsCache();
        }

        public ProjectManager CreateProjectManager()
        {
            return new ProjectManager(_projectManagerService, _aplayProjectsCache, _undoService, _undoManagerCache,
                _connectionFactory, _connectorFactory, _blockSymbolFactory,_sheetFactory);
        }
    }
}
