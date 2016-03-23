using System;
using APlayTest.Services.Infracstructure;
using sbardos.UndoFramework;
using Undo.Server;

namespace UndoTest.Server
{
    public class MyServer
    {
        private readonly IUndoService _undoService;
        private readonly APlayServer _server;
        private TaskManager _taskManager;

        public MyServer(IUndoService undoService)
        {
            _undoService = undoService;
             _server = new APlayServer();
        }

        public void Run()
        {
            _server.CloudReadyEventHandler += _server_CloudReadyEventHandler;
          
             _server.ClientConnectEventHandler += _server_ClientConnectEventHandler;
            _server.StartForClients("0.0.0.0:55555");   
        }

        void _server_ClientConnectEventHandler(Client NewDataClient__)
        {
            NewDataClient__.Id = IdGenerator.GetNextId();

            NewDataClient__.UndoManager = new UndoManager(_undoService, NewDataClient__.Id);
            NewDataClient__.TaskManager = _taskManager;
        }

        void _server_CloudReadyEventHandler()
        {
            _taskManager = new TaskManager(_undoService);
           
            //for (int i = 0; i < 10; i++)
            //{
            //    var id = IdGenerator.GetNextId();
            //    _taskManager.Tasks.Add(new Undo.Server.Task("Do something [" + id + "]", _undoService)
            //    {
            //        Id = id
            //    });
            //}
        }
    }
}