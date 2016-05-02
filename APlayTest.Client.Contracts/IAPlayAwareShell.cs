using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Services;

namespace APlayTest.Client.Contracts
{
    public interface IAPlayAwareShell : IShell
    {
        event EventHandler<Project> ProjectChanged;
        event EventHandler<UndoManager> UndoManagerChanged;
        Project Project { get; set; }
        UndoManager UndoManager { get;}
        Client Client { get; set; }
      
     
    }
}
