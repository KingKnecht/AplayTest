/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using APlayTest.Client;
namespace APlayTest.Client
{
  public class APlayClient : APlayTest.Client.APlayClientSkeleton
  {
    /// <summary>
    /// Constructor of the client role object. You could connect to a server here
    /// Note: don't create aplay objects before onCloudReady has been called!
    /// </summary>
    
    public APlayClient()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"APlayClient constructed","APlayTest.Client.APlayClient");
      this.Start("127.0.0.1:9999");
      /// TODO: add your code here
    }
    /// <summary>
    /// connected to server
    /// </summary>
    
    /// <param name="clientObject">
    /// the representation of this client
    /// </param>
    
    public override void onConnect(APlayTest.Client.Client clientObject)
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"APlayClient.onConnect called","APlayTest.Client.APlayClient");
      /// TODO: add your code here
    }
    /// <summary>
    /// disconnected from server
    /// </summary>
    
    public override void onDisconnect()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"APlayClient.onDisconnect called","APlayTest.Client.APlayClient");
      /// TODO: add your code here
    }
    /// <summary>
    /// client could not connect to server
    /// </summary>
    
    public override void onConnectionFailed()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"APlayClient.onConnectionFailed called","APlayTest.Client.APlayClient");
      /// TODO: add your code here
    }
  }
  
}
