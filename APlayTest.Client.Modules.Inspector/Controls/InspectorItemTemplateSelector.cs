using System.Windows;
using System.Windows.Controls;
using APlayTest.Client.Modules.Inspector.Inspectors;

namespace APlayTest.Client.Modules.Inspector.Controls
{
    public class InspectorItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LabelledTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ILabelledInspector)
                return LabelledTemplate;
            return DefaultTemplate;
        }
    }
}