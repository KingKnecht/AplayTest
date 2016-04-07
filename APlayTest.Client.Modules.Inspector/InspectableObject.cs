using System.Collections.Generic;
using APlayTest.Client.Modules.Inspector.Inspectors;

namespace APlayTest.Client.Modules.Inspector
{
    public class InspectableObject : IInspectableObject
    {
        public IEnumerable<IInspector> Inspectors { get; set; }

        public InspectableObject(IEnumerable<IInspector> inspectors)
        {
            Inspectors = inspectors;
        }
    }
}