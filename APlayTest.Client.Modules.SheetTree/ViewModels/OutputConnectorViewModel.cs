﻿using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    public class OutputConnectorViewModel : ConnectorViewModel
    {
        private readonly Func<BitmapSource> _valueCallback;

        public override ConnectorDirection ConnectorDirection
        {
            get { return ConnectorDirection.Output; }
        }

        private readonly BindableCollection<ConnectionViewModel> _connections;
        public IObservableCollection<ConnectionViewModel> Connections
        {
            get { return _connections; }
        }

        public BitmapSource Value
        {
            get { return _valueCallback(); }
        }

        public OutputConnectorViewModel(ElementViewModel element, string name, Color color, Func<BitmapSource> valueCallback)
            : base(element, name, color)
        {
            _connections = new BindableCollection<ConnectionViewModel>();
            _valueCallback = valueCallback;
        }
    }
}