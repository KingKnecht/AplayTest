using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using APlayTest.Client.Gemini.Framework.Controls;

namespace APlayTest.Client.Gemini.MainWindow.Views
{
    /// <summary>
    /// Interaction logic for ServerNotFoundView.xaml
    /// </summary>
    public partial class ServerNotFoundView : UserControl
    {
        private SmokeScreenAdorner _screenAdorner;

        public ServerNotFoundView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(this);
            _screenAdorner = new SmokeScreenAdorner(this);
            _screenAdorner.Name = "SmokeAdorner";
            _screenAdorner.Color = Colors.Black;
            _screenAdorner.Alpha = 95;
            layer.Add(_screenAdorner);
        }
    }
}
