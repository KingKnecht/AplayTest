using System.Windows;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public class ConnectionDragStartedEventArgs : ConnectionDragEventArgs
    {
        public object Connection { get; set; }

        public ConnectionDragStartedEventArgs(RoutedEvent routedEvent, object source,
            ElementItem elementItem)
            : base(routedEvent,source, elementItem)
        {
        }
    }

    public delegate void ConnectionDragStartedEventHandler(object sender, ConnectionDragStartedEventArgs e);
}