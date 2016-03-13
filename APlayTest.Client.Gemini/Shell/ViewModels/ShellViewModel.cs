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
using Gemini.Modules.UndoRedo.Services;

namespace APlayTest.Client.Gemini.Shell.ViewModels
{


    [Export(typeof(IShell))]
    [Export(typeof(IProjectAwareShell))]
    public class ShellViewModel : global::Gemini.Modules.Shell.ViewModels.ShellViewModel, IProjectAwareShell
    {
        static ShellViewModel()
        {
            ViewLocator.AddNamespaceMapping(typeof(ShellViewModel).Namespace, typeof(ShellView).Namespace);
        }

        public ShellViewModel()
            : base()
        {

        }

        public event EventHandler<Project> ProjectChanged;
        public void SetProject(Project project)
        {

            OnProjectChanged(project);
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

        protected override void OnActivate()
        {
            base.OnActivate();
        }
    }
}
