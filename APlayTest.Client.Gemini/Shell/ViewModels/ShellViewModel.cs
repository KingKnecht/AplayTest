using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APlayTest.Client.Contracts;
using APlayTest.Client.Gemini.Properties;
using Caliburn.Micro;
using Gemini.Framework.Services;
using Gemini.Modules.Shell.Views;
using Gemini.Modules.StatusBar.ViewModels;
using Gemini.Modules.UndoRedo.Services;

namespace APlayTest.Client.Gemini.Shell.ViewModels
{


    [Export(typeof(IShell))]
    [Export(typeof(IAPlayAwareShell))]
    public class ShellViewModel : global::Gemini.Modules.Shell.ViewModels.ShellViewModel, IAPlayAwareShell
    {
        private Project _project;
        private Client _client;

        static ShellViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(ShellViewModel).Namespace, typeof(ShellView).Namespace);
        }

        public event EventHandler<Project> ProjectChanged;
        
        public Project Project
        {
            get { return _project; }
            set
            {
                if (Equals(value, _project)) return;
                _project = value;
                OnProjectChanged(_project);

                StatusBar.Items.Clear();
                //StatusBar.AddItem(Project.ProjectDetail.Name, new GridLength(1, GridUnitType.Star));
                //StatusBar.AddItem(Project.ProjectDetail.CreatedBy, new GridLength(100));
                ////StatusBar.AddItem(Project.ProjectDetail.CreationDate.ToLongDateString(), new GridLength(100));
                NotifyOfPropertyChange(() => Project);
            }
        }

        public Client Client
        {
            get { return _client; }
            set
            {
                if (Equals(value, _client)) return;
                _client = value;
                StatusBar.Items.Clear();
                StatusBar.AddItem("Connected to " + Client.RemoteAddress, GridLength.Auto);
                NotifyOfPropertyChange();
            }
        }

        //public override void CanClose(Action<bool> callback)
        //{
        //    Coroutine.BeginExecute(CanClose().GetEnumerator(), null, (s, e) => callback(!e.WasCancelled));
        //}

        //private IEnumerable<IResult> CanClose()
        //{
        //    yield return new MessageBoxResult();
        //}

        //private class MessageBoxResult : IResult
        //{
        //    public event EventHandler<ResultCompletionEventArgs> Completed;

        //    public void Execute(CoroutineExecutionContext context)
        //    {
        //        var result = System.Windows.MessageBoxResult.Yes;

        //        //if (Settings.Default.ConfirmExit)
        //        {
        //            result = MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButton.YesNo);
        //        }

        //        Completed(this, new ResultCompletionEventArgs { WasCancelled = (result != System.Windows.MessageBoxResult.Yes) });
        //    }
        //}

        //protected override void OnViewLoaded(object view)
        //{
        //    base.OnViewLoaded(view);
        //}

        protected virtual void OnProjectChanged(Project e)
        {
            var handler = ProjectChanged;
            if (handler != null) handler(this, e);
        }
        
    }
}
