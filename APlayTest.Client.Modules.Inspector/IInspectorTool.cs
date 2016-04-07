using System;
using Gemini.Framework;

namespace APlayTest.Client.Modules.Inspector
{
	public interface IInspectorTool : ITool
	{
	    event EventHandler SelectedObjectChanged;
        IInspectableObject SelectedObject { get; set; }
	}
}