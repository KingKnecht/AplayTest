using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lobby.Server.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var lobbyServer = new Lobby.Server.APlayServer(20000);
        }
    }
}
