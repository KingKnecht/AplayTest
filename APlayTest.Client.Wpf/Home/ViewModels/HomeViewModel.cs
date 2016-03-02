using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APlayTest.Client.Wpf.Framework.Services;
using Caliburn.Micro;

namespace APlayTest.Client.Wpf.Home.ViewModels
{
    public class HomeViewModel : Screen, IDocument
    {
        public HomeViewModel()
        {
            DisplayName = "Home";
        }
        public Guid Id { get; private set; }
        public string ContentId { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public Uri IconSource { get; private set; }
        public bool IsSelected { get; set; }
        public bool ShouldReopenOnStart { get; private set; }
        public void LoadState(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void SaveState(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
