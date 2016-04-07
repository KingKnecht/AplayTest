using System;
using System.ComponentModel;
using APlayTest.Client.Modules.Inspector.Inspectors;

namespace APlayTest.Client.Modules.Inspector.Conventions
{
    public class EnumPropertyEditorBuilder : PropertyEditorBuilder
    {
        public override bool IsApplicable(PropertyDescriptor propertyDescriptor)
        {
            return typeof(Enum).IsAssignableFrom(propertyDescriptor.PropertyType);
        }

        public override IEditor BuildEditor(PropertyDescriptor propertyDescriptor)
        {
            return new EnumEditorViewModel(propertyDescriptor.PropertyType);
        }
    }
}