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

            var server = new APlayServer(Int32.Parse(Properties.Settings.Default.ServerPort),
                new ProjectManagerFactory(new ProjectManagerService(), undoService), undoService);

        }
    }
}
