using System;

namespace sbardos.UndoFramework
{
    public class ActiveStateChangedEventArgs : EventArgs
    {
        public ExternalChangeSet ChangeSet { get; private set; }
        public int ClientId { get; private set; }

        public ActiveStateChangedEventArgs(ExternalChangeSet changeSet, int clientId)
        {
            ChangeSet = changeSet;
            ClientId = clientId;
        }
    }
}