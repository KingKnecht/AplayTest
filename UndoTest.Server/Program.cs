using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UndoTest.Server
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var myServer = new MyServer();
            myServer.Run();
        }
    }
}
