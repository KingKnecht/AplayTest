using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using APlayTest.Client.Modules.SheetTree.ViewModels;
using APlayTest.Client.Modules.SheetTree.ViewModels.Elements;

namespace APlayTest.Client.Modules.SheetTree.TemplateSelectors
{
    public sealed class ElementsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;
            if (element == null)
            {
                return null;
            }

            if (item is BlockViewModel)
            {
               return  element.TryFindResource("BlockTemplate") as DataTemplate;
            }
            
            if (item is ConnectorViewModel)
            {
                return element.TryFindResource("ConnectorTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
