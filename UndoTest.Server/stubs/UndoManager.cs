/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using Undo.Server;
namespace Undo.Server
{
  public class UndoManager : Undo.Server.UndoManagerSkeleton
  {
    /// <summary>
    /// Use this constructor to create instances in your code.
    /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
    ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
    /// </summary>
    
    public UndoManager()
    {
      /// TODO: add your code here
    }
    public override void onStartTransaction()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"UndoManager.onStartTransaction called","Undo.Server.UndoManager");
      /// TODO: add your code here
    }
    public override void onEndTransaction()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"UndoManager.onEndTransaction called","Undo.Server.UndoManager");
      /// TODO: add your code here
    }
    public override void onCancelTransaction()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"UndoManager.onCancelTransaction called","Undo.Server.UndoManager");
      /// TODO: add your code here
    }
    public override void onExecuteUndo()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"UndoManager.onExecuteUndo called","Undo.Server.UndoManager");
      /// TODO: add your code here
    }
    public override void onExecuteRedo()
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"UndoManager.onExecuteRedo called","Undo.Server.UndoManager");
      /// TODO: add your code here
    }
  }
  
}