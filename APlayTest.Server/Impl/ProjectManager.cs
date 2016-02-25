/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using APlay.Generated.Intern.Server;
using APlayTest.Server;
using APlayTest.Services;

namespace APlayTest.Server
{
    public sealed class ProjectManager : APlayTest.Server.ProjectManagerSkeleton
    {
        private readonly IProjectDetailsService _projectDetailsService;
        private string _searchString;

        /// <summary>
        /// Use this constructor to create instances in your code.
        /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
        ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
        /// </summary>
        public ProjectManager()
        {
            /// TODO: add your code here
        }
        public ProjectManager(IProjectDetailsService projectDetailsService)
        {
            _projectDetailsService = projectDetailsService;

            var convertedDetails = _projectDetailsService.GetProjectDetails()
                .Select(d => new ProjectDetail(d.Name, d.CreatedBy, d.CreationDate, d.ProjectId));

            foreach (var convertedDetail in convertedDetails)
            {
                ProjectDetails.Add(convertedDetail);
            }
        }

     
        public override void onJoinProject(int projectId__)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "ProjectManager.onJoinProject called", "APlayTest.Server.ProjectManager");
            /// TODO: add your code here
        }
        public override void onCreateProject(String name__)
        {
            // Autogenerated log message for call
            APlay.Common.Logging.Logger.LogDesigned(2, "ProjectManager.onCreateProject called", "APlayTest.Server.ProjectManager");

            var newProjectDetail = _projectDetailsService.CreateProject(name__);

            var aplayProjectDetail = new ProjectDetail(newProjectDetail.Name, newProjectDetail.CreatedBy,
                newProjectDetail.CreationDate, newProjectDetail.ProjectId);

            ProjectDetails.Add(aplayProjectDetail);

            SelectedProject = aplayProjectDetail;

            UpdateStates();
        }

        private void PublishProjectDetailsList(IEnumerable<ProjectDetail> projectDetails)
        {
            var projectDetailses = projectDetails as IList<ProjectDetail> ?? projectDetails.ToList();

            //Update projects which now longer exist.
            var oldProjects = ProjectDetails.Except(projectDetailses);

            foreach (var projectDetail in oldProjects)
            {
                ProjectDetails.Remove(projectDetail);
            }

            //Add newly created projects.
            var newProjects = projectDetailses.Except(ProjectDetails);

            foreach (var projectDetail in newProjects)
            {
                ProjectDetails.Add(projectDetail);
            }
        }

        public override void onSearchProjects(string searchString__)
        {
            _searchString = searchString__;


            var convertedDetails = _projectDetailsService.GetProjectDetails()
                .Where(d =>
                {
                    if (!string.IsNullOrEmpty(searchString__))
                    {
                        return d.Name.StartsWith(searchString__,StringComparison.CurrentCultureIgnoreCase);
                    }

                    return true;
                })
                .Select(d => new ProjectDetail(d.Name, d.CreatedBy, d.CreationDate, d.ProjectId)).ToList();


            PublishProjectDetailsList(convertedDetails);

            UpdateStates();
        }

        private void UpdateStates()
        {

            if (_searchString != string.Empty && !ProjectDetails.Any())
            {
                CanJoinProject = false;
                CanCreateProject = true;
            }
            else if (SelectedProject.ProjectId != 0)
            {
                CanJoinProject = true;
                CanCreateProject = false;
            }
            else if (SelectedProject.ProjectId == 0)
            {
                CanJoinProject = false;
                CanCreateProject = false;
            }

        }

        public override void onSelectedProjectChange(ProjectDetail NewSelectedProject__)
        {
            base.onSelectedProjectChange(NewSelectedProject__);

            UpdateStates();
        }
    }

}
