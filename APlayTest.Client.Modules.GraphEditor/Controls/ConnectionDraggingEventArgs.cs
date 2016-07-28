using System.Windows;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public class ConnectionDraggingEventArgs : ConnectionDragEventArgs
    {
        private readonly object _connection;

        public object Connection
        {
            get { return _connection; }
        }

        internal ConnectionDraggingEventArgs(RoutedEvent routedEvent, object source,
            ElementItem elementItem, object connection)
            : base(routedEvent, source, elementItem)
        {
            _connection = connection;
        }
    }

    public delegate void ConnectionDraggingEventHandler(object sender, ConnectionDraggingEventArgs e);
}