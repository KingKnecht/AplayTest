using System.ComponentModel.Composition;
using System.Windows.Input;
using Gemini.Framework.Commands;

namespace APlayTest.Client.Modules.SheetTree.Commands
{
    [CommandDefinition]
    public class DrawConnectLineCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.DrawConnectLine";
        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Draw connect line"; }
        }

        public override string ToolTip
        {
            get { return "Draw connect line"; }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<DrawConnectLineCommandDefinition>(new KeyGesture(Key.C, ModifierKeys.Alt));
    }
}