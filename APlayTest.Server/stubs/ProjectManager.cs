/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using APlayTest.Server;
namespace APlayTest.Server
{
  public class ProjectManager : APlayTest.Server.ProjectManagerSkeleton
  {
    /// <summary>
    /// Use this constructor to create instances in your code.
    /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
    ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
    /// </summary>
    
    public ProjectManager()
    {
      /// TODO: add your code here
    }
    public override void onJoinProject(int projectId__)
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"ProjectManager.onJoinProject called","APlayTest.Server.ProjectManager");
      /// TODO: add your code here
    }
    public override void onCreateProject(String name__)
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"ProjectManager.onCreateProject called","APlayTest.Server.ProjectManager");
      /// TODO: add your code here
    }
    public override void onSearchProjects(String searchString__)
    {
      // Autogenerated log message for call
      APlay.Common.Logging.Logger.LogDesigned(2,"ProjectManager.onSearchProjects called","APlayTest.Server.ProjectManager");
      /// TODO: add your code here
    }
  }
  
}
