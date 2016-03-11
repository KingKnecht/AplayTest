﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APlayTest.Client.Contracts;
using APlayTest.Client.Modules.SheetTree.ViewModels;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Xceed.Wpf.AvalonDock.Properties;

namespace APlayTest.Client.Modules.SheetTree.Commands
{
    [CommandDefinition]
    public class ViewSheetTreeCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.SheetTree";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get
            {
                return "SheetTree";
                //return Resources.ViewErrorListCommandText;
            }
        }

        public override string ToolTip
        {
            get
            {
                return "SheetTree";
                //return Resources.ViewErrorListCommandToolTip;
            }
        }
    }

    [CommandHandler]
    public class ViewErrorListCommandHandler : CommandHandlerBase<ViewSheetTreeCommandDefinition>
    {
        private readonly IProjectAwareShell _shell;

        [ImportingConstructor]
        public ViewErrorListCommandHandler(IProjectAwareShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool<SheetTreeViewModel>();
            return TaskUtility.Completed;
        }
    }
}
