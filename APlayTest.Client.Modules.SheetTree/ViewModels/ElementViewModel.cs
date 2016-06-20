using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using APlayTest.Client.Modules.SheetTree.Factories;
using Caliburn.Micro;

namespace APlayTest.Client.Modules.SheetTree.ViewModels
{
    public abstract class ElementViewModel : PropertyChangedBase
    {
        private OutputConnectorViewModel _outputConnector;
   
        protected readonly IConnectionViewModelFactory ConnectionViewModelFactory;
        public event EventHandler OutputChanged;

        public const double PreviewSize = 100;

        public abstract double X { get; set; }
        public abstract double Y { get; set; }


        private string _name;

        [Browsable(false)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        private bool _isSelected;

        [Browsable(false)]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }
        
        private readonly BindableCollection<InputConnectorViewModel> _inputConnectors;
        private InputConnectorViewModel _selectedInputConnector;

        public IList<InputConnectorViewModel> InputConnectors
        {
            get { return _inputConnectors; }
        }

        public virtual InputConnectorViewModel SelectedInputConnector
        {
            get { return _selectedInputConnector; }
            set
            {
                if (Equals(value, _selectedInputConnector)) return;
                _selectedInputConnector = value;
                NotifyOfPropertyChange(() => SelectedInputConnector);
            }
        }


        public OutputConnectorViewModel OutputConnector
        {
            get { return _outputConnector; }
            set
            {
                _outputConnector = value;
                NotifyOfPropertyChange(() => OutputConnector);
            }
        }
        
        //This constructor is used i.e. for drag&drop (Activator.CreateInstance(..))
        protected ElementViewModel()
        {
            _inputConnectors = new BindableCollection<InputConnectorViewModel>();
        }

        protected ElementViewModel(IConnectionViewModelFactory connectionViewModelFactory)
            : this()
        {
            ConnectionViewModelFactory = connectionViewModelFactory;

            _name = GetType().Name;
        }

        protected void AddInputConnector(Connector connector)
        {
            var inputConnector = new InputConnectorViewModel(this, connector, ConnectionViewModelFactory);

            inputConnector.SourceChanged += (sender, e) => OnInputConnectorConnectionChanged();
            _inputConnectors.Add(inputConnector);
        }

        //protected void SetOutputConnector(string name, Color color)
        //{
        //    //OutputConnector = new OutputConnectorViewModel(this, name, color);
        //}

        protected virtual void OnInputConnectorConnectionChanged()
        {

        }

        protected virtual void RaiseOutputChanged()
        {
            EventHandler handler = OutputChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}