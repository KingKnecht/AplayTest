using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using APlayTest.Client.Wpf.Framework.Services;
using APlayTest.Client.Wpf.Home.ViewModels;
using APlayTest.Client.Wpf.ProjectSelection.ViewModels;
using Caliburn.Micro;
using Castle.MicroKernel.Lifestyle.Pool;

namespace APlayTest.Client.Wpf.Shell.ViewModels
{
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive,  IDisposable
    {
        private readonly IWindowManager _windowManager;
        public static APlayClient APlayClient;
        private readonly CompositeDisposable _cleanup;
        private ProjectSelectionViewModel _projectSelectionViewModel;
        private BindableCollection<ITool> _tools;

        public ShellViewModel()
        {

            _tools = new BindableCollection<ITool>();

            Items.Add(new HomeViewModel());
            Items.Add(new HomeViewModel());

            ((IActivate)this).Activate();
          
        }

        public IObservableCollection<ITool> Tools
        {
            get { return _tools; }
            
        }

        public Object ContentTree { get; set; }

        protected override void OnActivate()
        {
            base.OnActivate();

            ContentTree = new HomeViewModel();
            ActivateItem(Items.First());
        }


        public void Dispose()
        {
            _cleanup.Dispose();
        }
    }
}

