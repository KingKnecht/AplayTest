using System.Windows;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public class ElementItemDraggingEventArgs : RoutedEventArgs
    {
      
        public ElementItemDraggingEventArgs(RoutedEvent routedEvent, object source, double horizontalChange, double verticalChange, double positionX, double positionY) :
            base(routedEvent, source)
        {
            HorizontalChange = horizontalChange;
            VerticalChange = verticalChange;
            PositionX = positionX;
            PositionY = positionY;
        }

        public double HorizontalChange { get; private set; }
        public double VerticalChange { get; private set; }
        public double PositionX { get; private set; }
        public double PositionY { get; private set; }

    }

    public delegate void ElementItemDraggingEventHandler(object sender, ElementItemDraggingEventArgs e);
}