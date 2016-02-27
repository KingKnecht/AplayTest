﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Services.Annotations;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;

namespace APlayTest.Services
{


    public interface IProjectDetailsService : INotifyPropertyChanged
    {
        IEnumerable<ProjectDetail> GetProjectDetails(Func<ProjectDetail, bool> filter);
        ProjectDetail CreateProject(string name);
        bool IsValidName(string name);

        IObservableCache<ProjectDetail, int> ProjectDetailsRx { get; }

    }

    public class ProjectDetailsService : IProjectDetailsService, IDisposable
    {
        private readonly List<ProjectDetail> _projects = new List<ProjectDetail>();
        private int _nextProjectId;
       
        private readonly SourceCache<ProjectDetail, int> _sourceCache = new SourceCache<ProjectDetail, int>(pd => pd.ProjectId);
        private readonly CompositeDisposable _cleanup = new CompositeDisposable();
        
        public ProjectDetailsService()
        {

            for (int i = 0; i < 5; i++)
            {
                CreateProject("Dummy_" + i);
            }

            _cleanup.Add(_sourceCache);
        }


        public IEnumerable<ProjectDetail> GetProjectDetails(Func<ProjectDetail, bool> filter)
        {
            return _projects.Where(filter);
        }

        public ProjectDetail CreateProject(string name)
        {
            var newProjectDetails = new ProjectDetail()
            {
                CreatedBy = "Not impl.",
                CreationDate = DateTime.Now,
                Name = name,
                ProjectId = ++_nextProjectId
            };

            _projects.Add(newProjectDetails);

            _sourceCache.AddOrUpdate(newProjectDetails);

            return newProjectDetails;
        }

        public bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && _projects.All(pd => pd.Name != name);
        }

        public IObservableCache<ProjectDetail, int> ProjectDetailsRx
        {
            get { return _sourceCache.AsObservableCache(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _cleanup.Dispose();
        }
    }

    public class ProjectDetail
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int ProjectId { get; set; }
    }
}
