using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Client.Modules.SheetTree.Commands;
using Gemini.Framework.Menus;

namespace APlayTest.Client.Modules.SheetTree
{
    public static class MenuDefinitions
    {
        [Export] 
        public static MenuItemDefinition ViewErrorListMenuItem = new CommandMenuItemDefinition
            <ViewSheetTreeCommandDefinition>(
            Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 0);
    }
}
