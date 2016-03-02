using System.Windows;
using System.Windows.Media;
using APlayTest.Client.Wpf.Framework.Services;

namespace APlayTest.Client.Wpf.MainWindow.ViewModels
{
    public interface IMainWindow
    {
        WindowState WindowState { get; set; }
        double Width { get; set; }
        double Height { get; set; }

        string Title { get; set; }
        ImageSource Icon { get; set; }

        IShell Shell { get; }

     
    }

  
}