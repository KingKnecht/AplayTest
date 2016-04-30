using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplayTest.Client.Modules.UndoHistory.Commands;
using Gemini.Framework.Menus;



namespace AplayTest.Client.Modules.UndoHistory
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewUndoHistoryMenuItem = new CommandMenuItemDefinition
            <ViewHistoryCommandDefinition>(
            Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 0);
    }
}
