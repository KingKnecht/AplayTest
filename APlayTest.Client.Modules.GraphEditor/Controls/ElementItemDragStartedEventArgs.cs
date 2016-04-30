using System.Windows;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public class ElementItemDragStartedEventArgs : RoutedEventArgs
    {
        public bool Cancel { get; set; }

        internal ElementItemDragStartedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }
    }

    public delegate void ElementItemDragStartedEventHandler(object sender, ElementItemDragStartedEventArgs e);
}