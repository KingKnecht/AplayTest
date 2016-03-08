using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APlayTest.Client.Gemini.Properties;
using Caliburn.Micro;
using Gemini.Framework.Services;
using Gemini.Modules.Shell.Views;

namespace APlayTest.Client.Gemini.Shell.ViewModels
{
    //[Export(typeof(IShell))]
    //public class ShellViewModel : global::Gemini.Modules.Shell.ViewModels.ShellViewModel
    //{
    //    static ShellViewModel()
    //    {
    //        ViewLocator.AddNamespaceMapping(typeof(ShellViewModel).Namespace, typeof(ShellView).Namespace);
    //    }

    //    public override void CanClose(Action<bool> callback)
    //    {
    //        Coroutine.BeginExecute(CanClose().GetEnumerator(), null, (s, e) => callback(!e.WasCancelled));
    //    }

    //    private IEnumerable<IResult> CanClose()
    //    {
    //        yield return new MessageBoxResult();
    //    }

    //    private class MessageBoxResult : IResult
    //    {
    //        public event EventHandler<ResultCompletionEventArgs> Completed;

    //        public void Execute(CoroutineExecutionContext context)
    //        {
    //            var result = System.Windows.MessageBoxResult.Yes;

    //            //if (Settings.Default.ConfirmExit)
    //            {
    //                result = MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButton.YesNo);
    //            }

    //            Completed(this, new ResultCompletionEventArgs { WasCancelled = (result != System.Windows.MessageBoxResult.Yes) });
    //        }
    //    }

    //    protected override void OnViewLoaded(object view)
    //    {
    //        base.OnViewLoaded(view);
    //    }
    //}
}
