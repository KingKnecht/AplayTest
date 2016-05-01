using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APlayTest.Client.Modules.SheetTree.Commands;
using Gemini.Framework.Commands;
using Gemini.Framework.Menus;

namespace APlayTest.Client.Modules.SheetTree
{
    public static class MenuDefinitions
    {
        [Export] 
        public static MenuItemDefinition ViewSheetTreeMenuItem = new CommandMenuItemDefinition
            <ViewSheetTreeCommandDefinition>(
            Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 0);

        [Export]
        public static MenuItemDefinition ViewDeleteMenuItem = new CommandMenuItemDefinition
            <DeleteCommandDefinition>(
            Gemini.Modules.MainMenu.MenuDefinitions.EditUndoRedoMenuGroup, 0);

      
    }
   
}
