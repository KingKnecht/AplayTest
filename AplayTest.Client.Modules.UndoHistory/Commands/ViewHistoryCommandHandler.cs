using System.ComponentModel.Composition;
using System.Threading.Tasks;
using AplayTest.Client.Modules.UndoHistory.ViewModels;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;

namespace AplayTest.Client.Modules.UndoHistory.Commands
{
    [CommandHandler]
    public class ViewHistoryCommandHandler : CommandHandlerBase<ViewHistoryCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public ViewHistoryCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool<UndoHistoryViewModel>();
            return TaskUtility.Completed;
        }
    }
}
