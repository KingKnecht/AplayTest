using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using sbardos.UndoFramework;

namespace UndoTest.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var undoService = new UndoServiceFactory().Create();
            var myServer = new MyServer(undoService);

            myServer.Run();
        }
    }
}
