using System.ComponentModel.Composition;
using APlayTest.Client.Modules.Inspector.Commands;
using Gemini.Framework.Menus;

namespace APlayTest.Client.Modules.Inspector
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewInspectorMenuItem = new CommandMenuItemDefinition<ViewInspectorCommandDefinition>(
            Gemini.Modules.MainMenu.MenuDefinitions.ViewPropertiesMenuGroup, 1);
    }
}