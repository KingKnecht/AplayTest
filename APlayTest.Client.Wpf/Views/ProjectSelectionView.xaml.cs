using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APlayTest.Client.Wpf.Framework.Controls;

namespace APlayTest.Client.Wpf.Views
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
            layer.Add(_screenAdorner);
        }
    }
}
