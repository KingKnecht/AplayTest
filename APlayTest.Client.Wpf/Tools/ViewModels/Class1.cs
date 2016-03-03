using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APlayTest.Client.Wpf.Framework.Services;
using Caliburn.Micro;

namespace APlayTest.Client.Wpf.Tools.ViewModels
{

    public abstract class LayoutItemBase : Screen, ILayoutItem
    {
        private readonly Guid _id = Guid.NewGuid();

        //public abstract ICommand CloseCommand { get; }

        [Browsable(false)]
        public Guid Id
        {
            get { return _id; }
        }

        [Browsable(false)]
        public string ContentId
        {
            get { return _id.ToString(); }
        }

        [Browsable(false)]
        public virtual Uri IconSource
        {
            get { return null; }
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

        [Browsable(false)]
        public virtual bool ShouldReopenOnStart
        {
            get { return false; }
        }

        public virtual void LoadState(BinaryReader reader)
        {
        }

        public virtual void SaveState(BinaryWriter writer)
        {
        }
    }




    public abstract class Tool : LayoutItemBase, ITool
    {
      
        public abstract PaneLocation PreferredLocation { get; }

        public virtual double PreferredWidth
        {
            get { return 200; }
        }

        public virtual double PreferredHeight
        {
            get { return 200; }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }


        public override bool ShouldReopenOnStart
        {
            // Tool windows should always reopen on app start by default.
            get { return true; }
        }

        protected Tool()
        {
            IsVisible = true;
        }
    }

    public class InspectorViewModel : Tool
    {
        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Right; }
        }

        public override double PreferredWidth
        {
            get { return 300; }
        }
    }
}
