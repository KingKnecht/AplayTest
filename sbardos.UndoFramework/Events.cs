using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Undo.Client;

namespace sbardos.UndoFramework
{
    public enum StateChangeDirection
    {
        Redo,
        Undo
    }

    public class StackChangedEventArgs : EventArgs
    {
        public IList<HistoryChange> HistoryChanges { get; private set; }
        public int ActiveHistoryEntryId { get; private set; }
        public int ClientId { get; private set; }

        public StackChangedEventArgs(IList<HistoryChange> historyChanges, int activeHistoryEntryId, int clientId)
        {
            HistoryChanges = historyChanges;
            ActiveHistoryEntryId = activeHistoryEntryId;
            ClientId = clientId;
        }
    }
}
