using System.ComponentModel;
using APlayTest.Client.Modules.Inspector.Inspectors;

namespace APlayTest.Client.Modules.Inspector.Conventions
{
    public abstract class PropertyEditorBuilder
    {
        public abstract bool IsApplicable(PropertyDescriptor propertyDescriptor);
        public abstract IEditor BuildEditor(PropertyDescriptor propertyDescriptor);
    }
}