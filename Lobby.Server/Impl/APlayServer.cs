/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using Lobby.Server;
namespace Lobby.Server
{
    public class APlayServer : Lobby.Server.APlayServerSkeleton
    {
        /// <summary>
        /// You should initiate the server here.
        ///  
        /// Note: don't create aplay objects before onCloudReady has been called!
        /// </summary>
        /// <param name="port">Port used to listen to client connections</param>
        public APlayServer(int port)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "APlayServer constructed", "Lobby.Server.APlayServer");
            StartForClients("0.0.0.0:" + port);
        }
        /// <summary>
        /// a client connected
        /// </summary>

        /// <param name="client">
        /// the representation of this client
        /// </param>

        public override void onClientConnect(Lobby.Server.Client client)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "APlayServer.onClientConnect called", "Lobby.Server.APlayServer");
            /// TODO: add your code here
        }
        /// <summary>
        /// a client disconnected
        /// </summary>

        /// <param name="client">
        /// the representation of this client
        /// </param>

        public override void onClientDisconnect(Lobby.Server.Client client)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "APlayServer.onClientDisconnect called", "Lobby.Server.APlayServer");
            /// TODO: add your code here
        }

        public override void onCloudReady()
        {
            APlay.Common.Logging.Logger.LogDesigned(2, "APlayServer.onCloudReady called", "Lobby.Server.APlayServer");
            base.onCloudReady();
        }
    }

}
