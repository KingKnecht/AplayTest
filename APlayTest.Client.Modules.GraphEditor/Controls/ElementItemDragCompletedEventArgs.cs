using System.Windows;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public class ElementItemDragCompletedEventArgs : RoutedEventArgs
    {
        public ElementItemDragCompletedEventArgs(RoutedEvent routedEvent, object source) :
            base(routedEvent, source)
        {
        }
    }

    public delegate void ElementItemDragCompletedEventHandler(object sender, ElementItemDragCompletedEventArgs e);
}