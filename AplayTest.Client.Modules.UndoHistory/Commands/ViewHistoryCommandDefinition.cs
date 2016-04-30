using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplayTest.Client.Modules.UndoHistory.Properties;
using Gemini.Framework.Commands;
using Gemini.Properties;

namespace AplayTest.Client.Modules.UndoHistory.Commands
{
    [CommandDefinition]
    public class ViewHistoryCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.HistoryUndoRedo";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.ViewUndoHistoryCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.ViewUndoHistoryCommandTextToolTip; }
        }
    }
}
