using System.Windows;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public class ConnectionDragCompletedEventArgs : ConnectionDragEventArgs
    {
        private readonly object _connection;

        public object Connection
        {
            get { return _connection; }
        }

        internal ConnectionDragCompletedEventArgs(RoutedEvent routedEvent, object source, 
            ElementItem elementItem, object connection)
            : base(routedEvent, source, elementItem)
        {
            _connection = connection;
        }
    }

    public delegate void ConnectionDragCompletedEventHandler(object sender, ConnectionDragCompletedEventArgs e);
}