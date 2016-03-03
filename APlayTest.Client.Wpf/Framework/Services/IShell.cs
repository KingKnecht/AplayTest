using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;

namespace APlayTest.Client.Wpf.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
    {
        event EventHandler ActiveDocumentChanging;
        event EventHandler ActiveDocumentChanged;

        bool ShowFloatingWindowsInTaskbar { get; set; }

        //IMenu MainMenu { get; }
        //IToolBars ToolBars { get; }
        //IStatusBar StatusBar { get; }

        // TODO: Rename this to ActiveItem.
        ILayoutItem ActiveLayoutItem { get; set; }

        // TODO: Rename this to SelectedDocument.
        IDocument ActiveItem { get; }

        IObservableCollection<IDocument> Documents { get; }
        IObservableCollection<ITool> Tools { get; }

        //void ShowTool<TTool>() where TTool : ITool;
        //void ShowTool(ITool model);

        //void OpenDocument(IDocument model);
        //void CloseDocument(IDocument document);

        void Close();
    }

    public interface IDocument : ILayoutItem
    {
       // IUndoRedoManager UndoRedoManager { get; }
    }

    public interface ILayoutItem : IScreen
    {
        Guid Id { get; }
        string ContentId { get; }
        //ICommand CloseCommand { get; }
        Uri IconSource { get; }
        bool IsSelected { get; set; }
        bool ShouldReopenOnStart { get; }
        void LoadState(BinaryReader reader);
        void SaveState(BinaryWriter writer);
    }
}
