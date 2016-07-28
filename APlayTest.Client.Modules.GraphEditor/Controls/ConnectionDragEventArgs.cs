using System.Windows;

namespace APlayTest.Client.Modules.GraphEditor.Controls
{
    public abstract class ConnectionDragEventArgs : RoutedEventArgs
    {
        private readonly ElementItem _elementItem;

        public ElementItem ElementItem
        {
            get { return _elementItem; }
        }



        protected ConnectionDragEventArgs(RoutedEvent routedEvent, object source,
            ElementItem elementItem)
            : base(routedEvent, source)
        {
            _elementItem = elementItem;
        }
    }
}