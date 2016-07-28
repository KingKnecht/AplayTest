using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Server.Factories;
using APlayTest.Services;
using sbardos.UndoFramework;

namespace APlayTest.Server.Console
{
    public class Program
    {
        public static void Main()
        {
            var undoServiceFactory = new UndoServiceFactory();
            var undoService = undoServiceFactory.Create();
            var clientStateService = new ClientIdLookup();
            var undoManagerCache = new UndoManagerCache(undoService);
            
            var projectManagerService = new ProjectManagerService();
            

            var connectionFactory = new ConnectionFactory(undoService);
            var connectorFactory = new ConnectorFactory(undoService, connectionFactory);
            connectionFactory.ConnectorFactory = connectorFactory; //Make sure to inject the connectorFactory.

            var blockSymbolFactory = new BlockSymbolFactory(undoService, connectorFactory);
            var sheetFactory = new SheetFactory(undoService, connectionFactory, blockSymbolFactory, connectorFactory);
            
            connectionFactory.SheetFactory = sheetFactory;
            blockSymbolFactory.SheetFactory = sheetFactory;
            connectorFactory.SheetFactory = sheetFactory;

            var projectManager = new ProjectManagerFactory(projectManagerService, undoService, undoManagerCache,
                connectorFactory, connectionFactory, blockSymbolFactory, sheetFactory);

            var server = new APlayServer(Int32.Parse(Properties.Settings.Default.ServerPort), projectManager,
                clientStateService);
            
        }
    }
}
