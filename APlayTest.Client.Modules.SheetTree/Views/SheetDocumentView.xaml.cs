using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using APlayTest.Client.Modules.GraphEditor.Controls;
using APlayTest.Client.Modules.SheetTree.ViewModels;
using APlayTest.Client.Modules.SheetTree.ViewModels.Elements;
using Gemini.Modules.Toolbox;
using Gemini.Modules.Toolbox.Models;

namespace APlayTest.Client.Modules.SheetTree.Views
{
    /// <summary>
    /// Interaction logic for SheetDocumentView.xaml
    /// </summary>
    public partial class SheetDocumentView : UserControl
    {
        private Point _originalContentMouseDownPoint;

        public SheetDocumentView()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Xaml version of loading this template is not working...
            ResourceDictionary dict = new ResourceDictionary();
            Uri dataTemplatesAbsoluteUri = new Uri("pack://application:,,,/APlayTest.Client.Modules.SheetTree;component/DataTemplates/ElementDataTemplates.xaml", UriKind.Absolute);
            dict.Source = dataTemplatesAbsoluteUri;
            Application.Current.Resources.MergedDictionaries.Add(dict);


        }

        private SheetDocumentViewModel ViewModel
        {
            get { return (SheetDocumentViewModel)DataContext; }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            base.OnPreviewMouseDown(e);
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    if (e.Key == Key.Delete)
        //        ((SheetDocumentViewModel)DataContext).DeleteSelectedElements();
        //    base.OnKeyDown(e);
        //}

        private void OnGraphControlRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            _originalContentMouseDownPoint = e.GetPosition(GraphControl);
            GraphControl.CaptureMouse();
            Mouse.OverrideCursor = Cursors.ScrollAll;
            e.Handled = true;
        }

        private void OnGraphControlRightMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = null;
            GraphControl.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void OnGraphControlMouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed && GraphControl.IsMouseCaptured)
            {
                Point currentContentMousePoint = e.GetPosition(GraphControl);
                Vector dragOffset = currentContentMousePoint - _originalContentMouseDownPoint;

                ZoomAndPanControl.ContentOffsetX -= dragOffset.X;
                ZoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
        }

        private void OnGraphControlMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomAndPanControl.ZoomAboutPoint(
                ZoomAndPanControl.ContentScale + e.Delta / 1000.0f,
                e.GetPosition(GraphControl));

            e.Handled = true;
        }

        private void OnGraphControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.OnSelectionChanged();
        }

        private void OnGraphControlConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
        {
            var currentDragPoint = Mouse.GetPosition(GraphControl);
            var connection = ViewModel.OnConnectionDragStarted(currentDragPoint);
            e.Connection = connection;
        }

        private void OnGraphControlConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {

            var currentDragPoint = Mouse.GetPosition(GraphControl);
            var connection = (ConnectionViewModel)e.Connection;
            ViewModel.OnConnectionDragging(currentDragPoint, connection);
        }

        private void OnGraphControlConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var currentDragPoint = Mouse.GetPosition(GraphControl);
            var newConnection = (ConnectionViewModel)e.Connection;
            ViewModel.OnConnectionDragCompleted(currentDragPoint, newConnection);
        }

        private void OnGraphControlDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(ToolboxDragDrop.DataFormat))
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                var toolboxItem = (ToolboxItem)e.Data.GetData(ToolboxDragDrop.DataFormat);
                var mousePosition = e.GetPosition(GraphControl);

                if (toolboxItem.ItemType == typeof(BlockViewModel))
                {
                    var element = (BlockViewModel)Activator.CreateInstance(toolboxItem.ItemType);

                    element.X = mousePosition.X - BlockViewModel.PreviewSize / 2;
                    element.Y = mousePosition.Y - BlockViewModel.PreviewSize / 2;
                    element.IsSelected = true;

                    ViewModel.AddGhost(element);
                }
                else if (toolboxItem.ItemType == typeof(ConnectorViewModel))
                {
                    var element = (ConnectorViewModel)Activator.CreateInstance(toolboxItem.ItemType);

                    element.X = mousePosition.X - ConnectorViewModel.PreviewSizeX / 2;
                    element.Y = mousePosition.Y - ConnectorViewModel.PreviewSizeY / 2;
                    element.IsSelected = true;
                    
                    ViewModel.AddGhost(element);
                }
            }
        }

        private void OnGraphControlDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(ToolboxDragDrop.DataFormat))
            {
                var mousePosition = e.GetPosition(GraphControl);

                var toolboxItem = (ToolboxItem)e.Data.GetData(ToolboxDragDrop.DataFormat);

                if (toolboxItem.ItemType == typeof(BlockViewModel))
                {
                    var element = (BlockViewModel)Activator.CreateInstance(toolboxItem.ItemType);

                    element.X = mousePosition.X - BlockViewModel.PreviewSize / 2;
                    element.Y = mousePosition.Y - BlockViewModel.PreviewSize / 2;
                   
                    ViewModel.DropElement(element);
                }
                else if (toolboxItem.ItemType == typeof(ConnectorViewModel))
                {
                    var element = (ConnectorViewModel)Activator.CreateInstance(toolboxItem.ItemType);

                    element.X = mousePosition.X - ConnectorViewModel.PreviewSizeX / 2;
                    element.Y = mousePosition.Y - ConnectorViewModel.PreviewSizeY / 2;

                    ViewModel.DropElement(element);
                }

            }
        }

        private void OnGraphControlDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(ToolboxDragDrop.DataFormat))
            {
                var mousePosition = e.GetPosition(GraphControl);

                var ghost = ViewModel.GetGhost();
                if (ghost is BlockViewModel)
                {
                   
                    ((BlockViewModel)ghost).X = mousePosition.X - BlockViewModel.PreviewSize / 2;
                    ((BlockViewModel)ghost).Y = mousePosition.Y - BlockViewModel.PreviewSize / 2;

                }
                else if (ghost is ConnectorViewModel)
                {
                    ((ConnectorViewModel)ghost).X = mousePosition.X - ConnectorViewModel.PreviewSizeX / 2;
                    ((ConnectorViewModel)ghost).Y = mousePosition.Y - ConnectorViewModel.PreviewSizeY / 2;

                }
            }
        }

        private void OnElementItemDragStarted(object sender, ElementItemDragStartedEventArgs e)
        {
            var dataContext = ((ElementItem)e.OriginalSource).DataContext;
            var currentDragPoint = Mouse.GetPosition(GraphControl);


            var itemViewModel = (SymbolBaseViewModel)dataContext;

            ViewModel.OnElementItemDragStarted(itemViewModel, currentDragPoint);
        }

        private void OnElementItemDragCompleted(object sender, ElementItemDragCompletedEventArgs e)
        {
            var dataContext = ((ElementItem)e.OriginalSource).DataContext;
            var itemViewModel = (SymbolBaseViewModel)dataContext;
            var currentDragPoint = Mouse.GetPosition(GraphControl);

            ViewModel.OnElementItemDragCompleted(itemViewModel, currentDragPoint);
        }

        private void OnElementItemDragging(object sender, ElementItemDraggingEventArgs e)
        {
            var dataContext = ((ElementItem)e.OriginalSource).DataContext;
            var itemViewModel = (SymbolBaseViewModel)dataContext;

            ViewModel.OnElementItemDragging(itemViewModel, e.HorizontalChange,
               e.VerticalChange, e.PositionX, e.PositionY);
        }


        private Point GetContentPoint(Point zoomAndPanPoint, double scale)
        {
            return new Point(zoomAndPanPoint.X * (1.0 / scale), zoomAndPanPoint.Y * (1.0 / scale));
        }


    }
}
