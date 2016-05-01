using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Gemini.Framework;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public class ElementItem : ListBoxItem
    {
        private Point _lastMousePosition;
        private bool _isLeftMouseButtonDown;
        private bool _isDragging;

        static ElementItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementItem),
                new FrameworkPropertyMetadata(typeof(ElementItem)));
        }

        #region RoutedEvents


        //
        //DragStarted Routed Event
        //
        internal static readonly RoutedEvent DragStartedEvent = EventManager.RegisterRoutedEvent(
            "DragStarted", RoutingStrategy.Bubble, typeof(ElementItemDragStartedEventHandler), typeof(ElementItem));

        // Provide CLR accessors for the event
        internal event ElementItemDragStartedEventHandler DragStarted
        {
            add { AddHandler(DragStartedEvent, value); }
            remove { RemoveHandler(DragStartedEvent, value); }
        }

        // This method raises the DragStart event
        void RaiseDragStartedEvent(ElementItem elementItem)
        {
            ElementItemDragStartedEventArgs newEventArgs = new ElementItemDragStartedEventArgs(ElementItem.DragStartedEvent, elementItem);
            RaiseEvent(newEventArgs);
        }



        //
        //DragCompleted Routed Event
        //
        internal static readonly RoutedEvent DragCompletedEvent = EventManager.RegisterRoutedEvent(
            "DragCompleted", RoutingStrategy.Bubble, typeof(ElementItemDragCompletedEventHandler), typeof(ElementItem));

        // Provide CLR accessors for the event
        internal event ElementItemDragCompletedEventHandler DragCompleted
        {
            add { AddHandler(DragCompletedEvent, value); }
            remove { RemoveHandler(DragCompletedEvent, value); }
        }

        // This method raises the DragStart event
        void RaiseDragCompletedEvent(ElementItem elementItem)
        {
            ElementItemDragCompletedEventArgs newEventArgs = new ElementItemDragCompletedEventArgs(ElementItem.DragCompletedEvent, elementItem);
            RaiseEvent(newEventArgs);
        }


        //
        //Dragging Routed Event
        //
        internal static readonly RoutedEvent DraggingEvent = EventManager.RegisterRoutedEvent(
            "Dragging", RoutingStrategy.Bubble, typeof(ElementItemDraggingEventHandler), typeof(ElementItem));

        // Provide CLR accessors for the event
        internal event ElementItemDraggingEventHandler Dragging
        {
            add { AddHandler(DraggingEvent, value); }
            remove { RemoveHandler(DraggingEvent, value); }
        }

        // This method raises the DragStart event
        void RaiseDraggingEvent(ElementItem elementItem, double deltaX, double deltaY, double positionX, double positionY)
        {
            ElementItemDraggingEventArgs newEventArgs = new ElementItemDraggingEventArgs(ElementItem.DraggingEvent, elementItem, deltaX, deltaY,positionX,positionY);
            RaiseEvent(newEventArgs);
        }


        #endregion

        #region Dependency properties

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(ElementItem),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(ElementItem),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty ZIndexProperty = DependencyProperty.Register(
            "ZIndex", typeof(int), typeof(ElementItem),
            new FrameworkPropertyMetadata(0));

        private GraphControl _parentControl;
   
        public int ZIndex
        {
            get { return (int)GetValue(ZIndexProperty); }
            set { SetValue(ZIndexProperty, value); }
        }

        #endregion

        private GraphControl ParentGraphControl
        {
           // get { return _parentControl ?? (_parentControl = VisualTreeUtility.FindParent<GraphControl>(this)); }
            get { return VisualTreeUtility.FindParent<GraphControl>(this); }
        }

        #region Mouse input

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            BringToFront();
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            DoSelection();

            var parentGraphControl = ParentGraphControl;
            if (parentGraphControl != null)
                _lastMousePosition = e.GetPosition(parentGraphControl);

            _isLeftMouseButtonDown = true;

            e.Handled = true;

            base.OnMouseLeftButtonDown(e);
        }

        private void DoSelection()
        {
            var parentGraphControl = ParentGraphControl;
            if (parentGraphControl == null)
                return;

            parentGraphControl.SelectedElements.Clear();
            IsSelected = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                var newMousePosition = e.GetPosition(ParentGraphControl);
                var delta = newMousePosition - _lastMousePosition;

                X += delta.X;
                Y += delta.Y;

                RaiseDraggingEvent(this, delta.X, delta.Y, X, Y);
             
                _lastMousePosition = newMousePosition;
          
            }
            if (_isLeftMouseButtonDown)
            {
                if (!_isDragging)
                {
                    _isDragging = true;
                    RaiseDragStartedEvent(this);

                    CaptureMouse();
                }
            }

            e.Handled = true;

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_isLeftMouseButtonDown)
            {
                _isLeftMouseButtonDown = false;

                if (_isDragging)
                {
                    ReleaseMouseCapture();
                    _isDragging = false;
                    RaiseDragCompletedEvent(this);
                }
            }

            e.Handled = true;

            base.OnMouseLeftButtonUp(e);
        }

        #endregion

        internal void BringToFront()
        {
            var parentGraphControl = ParentGraphControl;
            if (parentGraphControl == null)
                return;

            var maxZ = parentGraphControl.GetMaxZIndex();
            ZIndex = maxZ + 1;
        }
    }
}