using System.ComponentModel.Composition;
using System.Windows.Input;
using Gemini.Framework.Commands;

namespace APlayTest.Client.Modules.SheetTree.Commands
{
    [CommandDefinition]
    public class DeleteCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.Delete";
        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Delete";  }
        }

        public override string ToolTip
        {
            get { return "Delete"; }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<DeleteCommandDefinition>(new KeyGesture(Key.Delete));
    }
}