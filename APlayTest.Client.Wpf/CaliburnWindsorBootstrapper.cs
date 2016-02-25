﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using APlayTest.Client.Wpf.ViewModels;

namespace APlayTest.Client.Wpf
{
    public class CaliburnWindsorBootstrapper : Caliburn.Micro.BootstrapperBase
    {
        private IWindsorContainer _container;

        public CaliburnWindsorBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {

            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());


            _container.Register(

                Component.For<IWindsorContainer>().Instance(_container),
                Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifestyleSingleton(),
                Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifestyleSingleton(),
              
                Classes.FromThisAssembly()
                    .InSameNamespaceAs<IShellViewModel>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleSingleton()
                );

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShellViewModel>();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Resolve(serviceType);
            }
            return _container.Resolve(key, serviceType);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _container.Dispose();
            base.OnExit(sender, e);
        }
    }
}
