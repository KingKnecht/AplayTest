/**
* automatically generated by APlay 2.0.2.1
* www.aplaypowered.com
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using APlay.Common;
using APlay.Common.Utils;
using APlay.Common.DataTypes;
using APlayTest.Client;
using APlayTest.Client.Annotations;
using DynamicData;
using Reactive.Bindings;

namespace APlayTest.Client
{
    public  class ProjectManager : APlayTest.Client.ProjectManagerSkeleton, IDisposable
    {
        private readonly SourceCache<ProjectDetail, int> _projectDetailsRx;
        private readonly CompositeDisposable _cleanup;
        /// <summary>
        /// Use this constructor to create instances in your code.
        /// Note: leave the APInitOb null. Aplay sets this object if initialized by aplay.
        ///  if you want to determine in the constructor if the object is user created or by aplay - check IsInitializedByAPlay
        /// </summary>

        public ProjectManager()
        {
            //Todo: Read that shit and think about it.
            //Solange es keine M�glichkeit gibt die �nderungen der AplayList direkt 
            //per Extension-Methode in einen SourceCache umzuwandeln findet die Umwandlung hier statt und nicht im VM.
            _projectDetailsRx = new SourceCache<ProjectDetail, int>(pd => pd.ProjectId);

            _projectDetailsRx.Edit(e =>
            {
                e.AddOrUpdate(ProjectDetails);
            });

            _cleanup = new CompositeDisposable(_projectDetailsRx);
        }

        public override void onJoinedProject(Project project)
        {
            IsJoinedToProject.Value = true;
        }

        public ReactiveProperty<bool> IsJoinedToProject { get; private set; }

   
        public IObservableCache<ProjectDetail, int> ProjectDetailsRx
        {
            get { return _projectDetailsRx.AsObservableCache(); }
        }


        #region List to SourceCache convertion. 
        //TODO: Einen allgemeinen Konverter/ExtensionMethod f�r AplayList -> SourceCache schreiben.

        public override void onProjectDetailsClear()
        {
            base.onProjectDetailsClear();
            _projectDetailsRx.Clear();
        }

        public override void onProjectDetailsAdd(ProjectDetail element)
        {
            base.onProjectDetailsAdd(element);
            _projectDetailsRx.AddOrUpdate(element);
        }

        public override void onProjectDetailsRemove(ProjectDetail element)
        {
            base.onProjectDetailsRemove(element);
            _projectDetailsRx.Remove(element);
        }

        public override void onProjectDetailsRemoveAt(int pos, ProjectDetail element)
        {
            base.onProjectDetailsRemoveAt(pos, element);
            _projectDetailsRx.Remove(element);
        }

        public override void onProjectDetailsInsertAt(int pos, ProjectDetail element)
        {
            base.onProjectDetailsInsertAt(pos, element);
            _projectDetailsRx.AddOrUpdate(element);
        }

        
        #endregion

    

        public void Dispose()
        {
            _cleanup.Dispose();
        }
    }

}
