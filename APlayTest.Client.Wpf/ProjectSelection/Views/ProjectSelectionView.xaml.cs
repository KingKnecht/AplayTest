using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using APlayTest.Client.Wpf.Framework.Controls;

namespace APlayTest.Client.Wpf.ProjectSelection.Views
{
    /// <summary>
    /// Interaction logic for ProjectSelectionView.xaml
    /// </summary>
    public partial class ProjectSelectionView : UserControl
    {
        private SmokeScreenAdorner _screenAdorner;

        public ProjectSelectionView()
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
