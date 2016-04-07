using APlayTest.Client.Modules.Inspector.Properties;
using Gemini.Framework.Commands;

namespace APlayTest.Client.Modules.Inspector.Commands
{
    [CommandDefinition]
    public class ViewInspectorCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.Inspector";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.ViewInspectorCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.ViewInspectorCommandToolTip; }
        }
    }
}