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
using APlayTest.Client.Gemini.Framework.Controls;

namespace APlayTest.Client.Gemini.MainWindow.Views
{
    /// <summary>
    /// Interaction logic for JoinProjectView.xaml
    /// </summary>
    public partial class JoinProjectView : UserControl
    {
        public JoinProjectView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(this);
            var screenAdorner = new SmokeScreenAdorner(this)
            {
                Name = "SmokeAdorner",
                Color = Colors.Black,
                Alpha = 95
            };
            layer.Add(screenAdorner);
        }
    }
}
