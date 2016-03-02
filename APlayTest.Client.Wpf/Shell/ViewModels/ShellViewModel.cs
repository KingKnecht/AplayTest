using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using APlayTest.Client.Wpf.Framework.Services;
using APlayTest.Client.Wpf.ProjectSelection.ViewModels;
using Caliburn.Micro;

namespace APlayTest.Client.Wpf.Shell.ViewModels
{
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell, IDisposable
    {
        private readonly IWindowManager _windowManager;
        public static APlayClient APlayClient;
        private readonly CompositeDisposable _cleanup;
        private ProjectSelectionViewModel _projectSelectionViewModel;
        public ShellViewModel()
        {
            
        }

     
        public event EventHandler ActiveDocumentChanging;
        public event EventHandler ActiveDocumentChanged;
        public bool ShowFloatingWindowsInTaskbar { get; set; }
        public ILayoutItem ActiveLayoutItem { get; set; }
        public IObservableCollection<IDocument> Documents { get; private set; }
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _cleanup.Dispose();
        }
    }
}

