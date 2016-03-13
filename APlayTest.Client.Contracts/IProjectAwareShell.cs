using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Services;

namespace APlayTest.Client.Contracts
{
    public interface IProjectAwareShell : IShell
    {
        event EventHandler<Project> ProjectChanged;
        void SetProject(Project project);
    }
}
