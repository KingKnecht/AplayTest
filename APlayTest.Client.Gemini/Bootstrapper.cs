using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APlayTest.Client.Factories;
using APlayTest.Client.Gemini.MainWindow.ViewModels;
using Caliburn.Micro;
using Gemini;
using Gemini.Framework.Services;

namespace APlayTest.Client.Gemini
{
    public class Bootstrapper : AppBootstrapper
    {
        
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
           DisplayRootViewFor<IMainWindowEx>();
        }


        protected override void BindServices(CompositionBatch batch)
        {
            base.BindServices(batch);

            batch.AddExportedValue<IAplayClientFactory>(new AplayClientFactory());
        }
    }
}
